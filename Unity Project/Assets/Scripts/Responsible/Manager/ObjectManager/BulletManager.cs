using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    // 弾のPrefab
    [SerializeField]
    private GameObject bulletPrefab = null;
    // アクティブな弾用のリスト
    [SerializeField]
    private Stack<GameObject> activateBulletObjects = new Stack<GameObject>();
    // 非アクティブな弾用のリスト
    [SerializeField]
    private Stack<GameObject> inactivateBulletObjects = new Stack<GameObject>();
    // 弾の親オブジェクト
    private GameObject bulletParent;
    [SerializeField]
    private int bulletWayCountWhenRain = 3;

    // Start is called before the first frame update
    void Start()
    {
        bulletParent = GameObject.Find("Bullets").gameObject;
        // 親オブジェクトにセット
        foreach(var bulletObj in activateBulletObjects)
        {
            bulletObj.transform.SetParent(bulletParent.transform);
        }
    }

    /// <summary>
    /// 通常時、どの弾を使うのか決めるメソッド
    /// </summary>
    /// <param name="playerObj">弾を撃ったプレイヤー</param>
    public void UseBullet(GameObject playerObj)
    {
        // 未使用の弾があるなら使う
        if (inactivateBulletObjects.Count > 0)
        {
            var useBullet = inactivateBulletObjects.Pop();
            // 使用中の弾に登録
            activateBulletObjects.Push(useBullet);
            // アクティブ化
            Activate(playerObj, useBullet);
        }
        // 未使用の弾がないなら新しい弾を作成
        else
        {
            var useBullet = Instantiate(bulletPrefab);
            // 親オブジェクトを設定
            useBullet.transform.SetParent(bulletParent.transform);
            // 位置やスケールを初期化
            useBullet.transform.localPosition = Vector3.zero;
            useBullet.transform.localScale = new Vector3(1, 1, 1);
            // 使用中の弾に登録
            activateBulletObjects.Push(useBullet);
            // アクティブ化
            Activate(playerObj, useBullet);
        }
    }

    /// <summary>
    /// 雨天時、どの弾を使うのか決めるメソッド
    /// </summary>
    /// <param name="playerObj">弾を撃ったプレイヤー</param>
    public void UseBulletWhenRain(GameObject playerObj)
    {
        // 使用する弾の配列
        GameObject[] useBullets = new GameObject[bulletWayCountWhenRain];
        // 雨天時の弾のWayの数分ループ
        for(int i=0;i<bulletWayCountWhenRain;i++)
        {
            // 未使用の弾があるなら使う
            if (inactivateBulletObjects.Count > 0)
            {
                var useBullet = inactivateBulletObjects.Pop();
                // 使用中の弾に登録
                activateBulletObjects.Push(useBullet);
                // 使用する弾の配列に格納
                useBullets[i] = useBullet;
            }
            // 未使用の弾がないなら新しい弾を作成
            else
            {
                var useBullet = Instantiate(bulletPrefab);
                // 親オブジェクトを設定
                useBullet.transform.SetParent(bulletParent.transform);
                // 位置やスケールを初期化
                useBullet.transform.localPosition = Vector3.zero;
                useBullet.transform.localScale = new Vector3(1, 1, 1);
                // 使用中の弾に登録
                activateBulletObjects.Push(useBullet);
                // 使用する弾の配列に格納
                useBullets[i] = useBullet;
            }
        }
        // アクティブ化
        Activate(playerObj, useBullets);
    }

    /// <summary>
    /// 弾の発射処理をする関数
    /// </summary>
    /// <param name="playerObj">弾を発射したプレイヤー</param>
    private void Activate(GameObject playerObj,GameObject bulletObj)
    {
        // 各コンポーネントを取得
        var player=playerObj.GetComponent<Player>();
        var playerAttack = playerObj.GetComponent<PlayerAttack>();
        var bullet = bulletObj.GetComponent<Bullet>();
        var position = playerObj.transform.position;
        // 弾が出る位置をずらす
        position.y -= (playerAttack.offsetY - 0.2f);
        position.x += playerAttack.offsetX;
        // 撃ったプレイヤーの座標に合わせる
        bulletObj.transform.position = position;
        // 弾のスピードを決定
        bullet.speed = playerAttack.speed;
        // 弾を撃つ方向を決める
        bullet.bulletDirection = Bullet.BulletDirection.MIDDLE;
        // 弾を撃ったプレイヤーのIDを記憶
        bullet.player = player;
        // 弾を表示
        bulletObj.SetActive(true);
        // ショット関数を呼ぶ
        bullet.Shot();
        return;
    }

    /// <summary>
    /// 雨の時の弾の発射処理をする関数
    /// </summary>
    /// <param name="playerObj">弾を発射したプレイヤー</param>
    private void Activate(GameObject playerObj,GameObject[] bulletObjs)
    {
        for (int i = 0; i < 3; i++)
        {
            // 各コンポーネントを取得
            var player = playerObj.GetComponent<Player>();
            var playerAttack = playerObj.GetComponent<PlayerAttack>();
            var bullet = bulletObjs[i].GetComponent<Bullet>();
            // プールに弾があったら
            var position = playerObj.transform.position;
            // 弾が出る位置をずらす
            position.y -= (playerAttack.offsetY - 0.2f);
            position.x += playerAttack.offsetX;
            // 撃ったプレイヤーの座標に合わせる
            bulletObjs[i].transform.position = position;
            // 弾のスピードを決定
            bullet.speed = playerAttack.speed;
            // 弾を撃つ方向を決定
            bullet.bulletDirection = (Bullet.BulletDirection)i;
            // 弾を撃つ角度を決定
            switch (bullet.bulletDirection)
            {
                case Bullet.BulletDirection.UP:
                    bullet.upVec = playerAttack.upVec;
                    break;
                case Bullet.BulletDirection.DOWN:
                    bullet.downVec = playerAttack.downVec;
                    break;
            }
            // 雨フラグをONにする
            bullet.isRain = true;
            // 弾を撃ったプレイヤーのIDを記憶
            bullet.player = player;
            // 弾を表示
            bulletObjs[i].SetActive(true);
            // ショット関数を呼ぶ
            bullet.Shot();
        }
    }

    /// <summary>
    /// 撃った弾をプールに戻す関数
    /// </summary>
    /// <param name="bulletObj">プールに戻す弾</param>
    public void ReturnBullet(GameObject bulletObj)
    {
        // 非アクティブなListに戻す
        activateBulletObjects.Pop();
        inactivateBulletObjects.Push(bulletObj);
        // 非表示にする
        bulletObj.SetActive(false);
        // 位置初期化
        bulletObj.transform.position = new Vector3(0, 0, 0);
        // 大きさを初期化
        bulletObj.transform.localScale = new Vector3(1, 1, 1);
        var bullet = bulletObj.GetComponent<Bullet>();
        // 雨天時フラグをOffにする
        bullet.isRain = false;
        // 移動量初期化
        bullet.GetComponent<Bullet>().nowMoveVecY = 0;
    }
}

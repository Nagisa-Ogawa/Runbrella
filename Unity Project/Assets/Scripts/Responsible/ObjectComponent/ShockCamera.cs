using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockCamera : MonoBehaviour
{
    // カメラを揺らす時間
    [SerializeField]
    private float time = 0f;
    // カメラを揺らすパワー
    [SerializeField]
    private Vector2 power = new Vector2(0, 0);

    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "Camera";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> shockCameraDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out shockCameraDictionary);
        try
        {
            // ファイル読み込み
            time = shockCameraDictionary["プレイヤー死亡時のカメラの揺れる時間"];
            power.x = shockCameraDictionary["プレイヤー死亡時のカメラ横(X)方向の揺れの強さ"];
            power.y = shockCameraDictionary["プレイヤー死亡時のカメラ縦(Y)方向の揺れの強さ"];
        }
        catch
        {
            Debug.Assert(false, nameof(ShockCamera) + "でエラーが発生しました");
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        // textからパラメータを読み込む
        ReadTextParameter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// カメラを振動させるコルーチンを開始する関数
    /// </summary>
    public void StartShock()
    {
        // コルーチン開始
        StartCoroutine(Shock());
    }


    /// <summary>
    /// カメラを振動させるコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shock()
    {
        // 現在の時間
        float nowTime = 0;
        while (true)
        {
            var pos = gameObject.transform.position;
            // 乱数でどの方向に揺らすかを決める
            var x = pos.x + Random.Range(-1f, 1f) * power.x;
            var y = pos.y + Random.Range(-1f, 1f) * power.y;
            // 決めた座標をカメラにセット
            gameObject.transform.position = new Vector3(x, y, transform.position.z);
            // 規定秒数以上たったら終了
            if (nowTime > time)
            {
                // カメラのポジションを中心に戻す
                gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                yield break;
            }
            // 経過時間を加算
            nowTime += Time.deltaTime;
            yield return null;
        }
    }
}

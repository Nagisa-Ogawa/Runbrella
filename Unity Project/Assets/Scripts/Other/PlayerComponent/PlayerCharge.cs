using ResultScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharge : MonoBehaviour
{
    // チャージ時間
    float chargeTime = 0.0f;
    // チャージ数
    public int chargeCount = 0;
    // 1回チャージするまでの時間
    float oneChargeTime = 1.0f;

    // 必要なコンポーネント
    Player player;
    PlayerAttack playerAttack;
    PlayerBoost playerBoost;
    // 子オブジェクトのコンポーネント
    ChargeGauge chargeGauge;
    // チャージを開始するまでの秒数
    private float chargeStartTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        player = GetComponent<Player>();
        playerAttack = GetComponent < PlayerAttack>();
        playerBoost = GetComponent<PlayerBoost>();
        // チャージ状態を表すゲージオブジェクト
        var chargeGauge = transform.Find("ChargeGauge").gameObject;
        // ゲージのオブジェクトからのアニメーターコンポーネントの取得
        this.chargeGauge = chargeGauge.GetComponent<ChargeGauge>();
        // パラメータをセット
        ReadTextParameter();
    }

    /// <summary>
    /// テキストからパラメータを読み込む
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "";
        switch (player.charType)
        {
            case GameManager.CHARTYPE.PlayerA:
                textName = "Chara_A";
                break;
            case GameManager.CHARTYPE.PlayerB:
                textName = "Chara_B";
                break;
        }
        try
        {
            // テキストの中のデータをセットするディクショナリー
            SheetToDictionary.Instance.TextToDictionary(textName, out var chargeDictionary);

            try
            {
                // ファイル読み込み
                oneChargeTime = chargeDictionary["1ゲージチャージする秒数"];
                chargeStartTime = chargeDictionary["チャージを開始するまでの秒数"];
            }
            catch
            {
                Debug.Assert(false, nameof(PlayerCharge) + "でエラーが発生しました");
            }
        }
        catch
        {
            Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から"
                + textName + "の読み込みに失敗しました。");
        }
    }


    private void Update()
    {
        chargeGauge.SetChargeSprite(chargeCount);
    }

    /// <summary>
    /// チャージ処理
    /// </summary>
    private void Charge()
    {
        // チャージが出来ないステートなら
        if (player.IsIdle || player.IsBoost || player.IsDown || player.IsAfterGoal)
        {
            return;
        }
        // プレイヤーのチャージが出来るなら
        if(chargeCount < playerAttack.NowBulletCount)
        {
            //前回フレームのチャージ数を保存
            var beforeChargeCount = chargeCount;
            // 経過時間を計測
            chargeTime += Time.deltaTime;
            // チャージ数を計算
            chargeCount = (int)(chargeTime / oneChargeTime);
            // チャージの時間が開始時間を超えていたなら
            if(chargeTime > chargeStartTime)
            {
                // エフェクトをONにする
                player.PlayEffect(player.chargeingEffect);
                // チャージが停止中のエフェクトをOFFにする
                ChargePauseEffectStop();
                // 今回のフレームでチャージされたなら
                if (chargeCount > beforeChargeCount)
                {
                    // エフェクトをONにする。
                    player.PlayEffect(player.chargeSignal);
                }
            }
        }
        else
        {
            // エフェクトを一時停止する。
            ChargeStop();
        }
        // チャージが出来ない、又はチャージが完了したら
        if (chargeCount >= playerAttack.NowBulletCount)
        {
            // チャージ数をゲージのエネルギー数に合わせる
            chargeCount = playerAttack.NowBulletCount;
            // チャージ時間を計算
            chargeTime = chargeCount * oneChargeTime;
        }
    }

    /// <summary>
    /// チャージが一時停止中のエフェクトを停止する
    /// </summary>
    private void ChargePauseEffectStop()
    {
        if (player.chargePauseEffect.isPlaying == true)
        {
            // エフェクトを停止する
            player.StopEffect(player.chargePauseEffect);
            // すでに出ているエフェクトを非表示にする
            player.chargePauseEffect.Clear();
        }
        if(player.chargeMaxEffect.isPlaying == true)
        {
            // エフェクトを停止する
            player.StopEffect(player.chargeMaxEffect);
            // すでに出ているエフェクトを非表示にする
            player.chargeMaxEffect.Clear();
        }
    }

    /// <summary>
    /// チャージエフェクトの停止
    /// </summary>
    public void ChargeStop()
    {
        // エフェクトを一時停止する。
        player.StopEffect(player.chargeingEffect);
        player.chargeingEffect.Clear();
        if(chargeCount > 0 && chargeCount < 5)
        {
            // チャージが一時停止中の場合用のエフェクトを再生する
            player.PlayEffect(player.chargePauseEffect);
        }
        else if(chargeCount >= 5)
        {
            // チャージがMAXの場合のエフェクトを再生する
            player.PlayEffect(player.chargeMaxEffect);
        }
        // チャージ時間を計算
        chargeTime = chargeCount * oneChargeTime;
    }

    /// <summary>
    /// ブースト出来るかチェックして、チャージ数をブーストに反映させる
    /// </summary>
    /// <returns>ブースト出来るかできないか</returns>
    private bool BoostCheck()
    {
        // チャージの時間がチャージの開始時間より短かったら
        if(chargeTime < chargeStartTime)
        {
            // 攻撃できる状態なら
            if(!player.IsSlide && !player.IsGlide)
            {
                // 攻撃
                playerAttack.Attack();
            }
        }

        // チャージされていて、かつエネルギー以下なら
        if (chargeCount > 0 && chargeCount <= playerAttack.NowBulletCount)
        {
            // チャージ数をブーストに伝える
            playerBoost.GaugeCount = chargeCount;
            // チャージをリセット
            ChargeReset();
            return true;
        }
        else
        {
            // チャージをリセット
            ChargeReset();
            return false;
        }
    }

    /// <summary>
    /// 現在のチャージをリセット
    /// </summary>
    public void ChargeReset()
    {
        chargeCount = 0;
        chargeTime = 0.0f;
        // エフェクトが一時停止されている可能性があるので一度再生してから停止する
        ChargePauseEffectStop();
        player.StopEffect(player.chargeingEffect);
        player.StopEffect(player.chargeSignal);
    }

    /// <summary>
    /// ブーストのキーの入力を確認する
    /// </summary>
    /// <param name="controllerNo"></param>
    public void BoostKeyCheck()
    {
        // キーを長押ししたなら
        if (player.IsCharge)
        {
            // チャージする
            Charge();
        }
        // キーを離したなら
        else if (player.IsBoostStart)
        {
            // ブーストが出来ないステートなら
            if (player.IsIdle || player.IsBoost || player.IsDown || player.IsAfterGoal)
            {
                // チャージをリセットする
                ChargeReset();
                return;
            }
            else
            {
                // ブースト出来るなら
                if (BoostCheck())
                {
                    // ブーストを開始する
                    player.BoostStart();
                }
            }
        }
    }
}
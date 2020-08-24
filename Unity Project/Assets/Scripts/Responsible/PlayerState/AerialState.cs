using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : MonoBehaviour, IState
{
    // 必要なコンポーネント
    private PlayerAerial playerAerial;
    private PlayerSlide playerSlide;
    private PlayerAttack playerAttack;
    private PlayerCharge playerCharge;
    private Player character;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントを取得
        playerAerial = GetComponent<PlayerAerial>();
        playerSlide = GetComponent<PlayerSlide>();
        playerAttack = GetComponent<PlayerAttack>();
        playerCharge = GetComponent<PlayerCharge>();
        character = GetComponent<Player>();
    }

    /// <summary>
    /// ステートの開始処理
    /// </summary>
    void IState.Start()
    {
        // 滑空開始処理
        playerAerial.StartAerial();
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    void IState.Update()
    {
        // ジャンプボタンが押されたら
        if (character.IsGlideStart == true)
        {
            // 滑空状態に移行
            character.GlideStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
            return;
        }
        // 着地したら
        if (character.IsGround == true)
        {
            // ラン状態に移行
            character.RunStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
            return;
        }

        // アクションボタンが押されたら
        if (character.IsSlideStart == true)
        {
            // 手すりをつかむ猶予時間
            var catchSliderTime = playerSlide.catchSliderTime;
            // 手すりヒット判定
            playerSlide.RayTimerStart(catchSliderTime);
            // 保留　演出の終了
            // playerSlide.EffectOff();
            // チャージ演出を一時停止する
            playerCharge.ChargeStop();
        }

        // ショットボタンが押されたら
        if (character.IsAttack == true)
        {
            playerAttack.Attack();
        }

        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // ダウン状態に移行
            character.Down();
            return;
        }
        // ブーストのキー入力を確認
        playerCharge.BoostKeyCheck();
    }

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    void IState.FixedUpdate()
    {
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        playerAerial.Aerial();        
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    void IState.Exit()
    {
        // 滑空開始処理
        playerAerial.EndAerial();
    }
}

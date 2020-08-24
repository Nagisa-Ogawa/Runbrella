using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MonoBehaviour, IState
{
    // 必要なコンポーネント
    private PlayerRun playerRun;
    private PlayerSlide playerSlide;
    private PlayerAerial playerAerial;
    private PlayerAttack playerAttack;
    private PlayerJump playerJump;
    private PlayerGlide playerGlide;
    private PlayerCharge playerCharge;
    private Player character;
    private Rigidbody2D playerRigidbody2D;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントの取得
        playerRun = GetComponent<PlayerRun>();
        playerSlide = GetComponent<PlayerSlide>();
        playerAerial = GetComponent<PlayerAerial>();
        playerAttack = GetComponent<PlayerAttack>();
        playerJump = GetComponent<PlayerJump>();
        playerCharge = GetComponent<PlayerCharge>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerGlide = GetComponent<PlayerGlide>();
        character = GetComponent<Player>();
    }

    void IState.Start()
    {
        // 重力を初期化
        playerRigidbody2D.gravityScale = playerAerial.aerialGravityScale;
        // 角度を初期化
        transform.localRotation = Quaternion.identity;
        // 滑空中ホップ可能フラグをtrueにする
        playerGlide.CanHop = true;
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    void IState.Update()
    {
        //　ジャンプボタンが押されたら
        if (character.IsJumpStart == true)
        {
            //　ジャンプ
            playerJump.Jump();
            // 空中状態に移行
            character.AerialStart();

        }

        // アクションボタンが押されたら
        if (character.IsSlideStart == true)
        {
            // 手すりをつかむ猶予時間
            var catchSliderTime = playerSlide.catchSliderTime;
            // 手すりヒット判定
            playerSlide.RayTimerStart(catchSliderTime);
        }

        // 地面から落ちたら
        if (character.IsGround == false)
        {
            // 空中状態に移行
            character.AerialStart();
        }

        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // ダウン状態に移行
            character.Down();
        }

        // アタックボタンが押されたら
        if (character.IsAttack == true)
        {
            playerAttack.Attack();
        }

        // ブーストのキー入力を確認
        playerCharge.BoostKeyCheck();
    }

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    void IState.FixedUpdate()
    {
        playerRun.Run();
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    void IState.Exit()
    {
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideState : MonoBehaviour, IState
{
    // コンポーネント
    private PlayerGlide playerGlide;
    private Player character;
    private AerialState aerialState;
    private RunState runState;
    private DownState downState;
    private PlayerCharge playerCharge;
    private PlayerAttack playerAttack;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントを取得
        playerGlide = GetComponent<PlayerGlide>();
        character = GetComponent<Player>();
        aerialState = GetComponent<AerialState>();
        runState = GetComponent<RunState>();
        downState = GetComponent<DownState>();
        playerCharge = GetComponent<PlayerCharge>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    void IState.Start()
    {
        // 滑空開始処理
        playerGlide.StartGlide();
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    void IState.Update()
    {
        // ジャンプボタンが離されたら
        if (character.IsGlideEnd == true)
        {
            // 空中状態に移行
            character.AerialStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
        }

        // 地面についたら
        if (character.IsGround == true)
        {
            // ラン状態に移行
            character.RunStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
        }
        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // ダウン状態に移行
            character.Down();
        }
        // ブーストのキー入力を確認
        playerCharge.BoostKeyCheck();
    }

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    void IState.FixedUpdate()
    {       
        // 滑空中処理
        playerGlide.Gride();       
    }
    
    /// <summary>
    /// ステートの終了処理
    /// </summary>
    void IState.Exit()
    {
        // 滑空終了処理
        playerGlide.EndGlide();
    }
}

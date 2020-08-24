using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownState : MonoBehaviour,IState
{
    // 必要なコンポーネント
    private PlayerDown playerDown;
    private Player character;
    private RunState runState;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントの取得
        playerDown = GetComponent<PlayerDown>();
        character = GetComponent<Player>();
        runState = GetComponent<RunState>();

    }

    /// <summary>
    /// ステートの開始処理
    /// </summary>
    void IState.Start()
    {
        playerDown.StartDown();
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    void IState.Update()
    {
        // 一定時間経過したらダウン状態解除
        if (playerDown.
            TimeCounter(character.downTime))
        {
            // 地面に接地しているなら
            if (character.IsGround == true)
            {
                // 走り状態に移行
                character.RunStart();
            }
            else
            {
                // 空中状態に移行
                character.AerialStart();
            }
        }
    }

    /// <summary>
    /// 物理挙動のフレーム更新処理
    /// </summary>
    void IState.FixedUpdate()
    {
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    void IState.Exit()
    {
        // 終了処理
        playerDown.EndDown();
    }
}

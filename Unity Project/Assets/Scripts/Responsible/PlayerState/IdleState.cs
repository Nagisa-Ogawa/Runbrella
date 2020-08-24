using ResultScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour,IState
{
    // タイムライン中かどうかをチェックするキャラクター
    private Player player;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントの取得
        player = GetComponent<Player>();
    }
    void IState.Start(){ }

    void IState.Update(){ }

    void IState.FixedUpdate(){ }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    void IState.Exit()
    {
        // タイムラインの終了をキャラクターに知らせる
        player.IsTimeLine = false;    
    }
}

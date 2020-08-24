using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : IContext
{
    private IState state;

    public PlayerStateManager()
    {
    }

    // 現在のステートのUpdate()を呼び出す
    public void Update()
    {
        if (state == null)
        {
            return;
        }
        state.Update();
    }

    // 現在のステートのFixedUpdate()を呼び出す
    public void FixedUpdate()
    {
        if (state == null)
        {
            return;
        }
        state.FixedUpdate();
    }

    // 現在のステートを変更する
    public void ChangeState(IState state)
    {
        // nullチェック
        if (state == null)
        {
            Debug.Log("新しいStateがnullでした");
            return;
        }
        if (this.state != null)
        {
            // 現在のステートのExitを呼び出す
            this.state.Exit();
        }
        // ステートを切り替える
        this.state = state;
        // 新しいステートのStartを呼び出す
        this.state.Start();
    }

    public IState GetState()
    {
        return state;
    }

}

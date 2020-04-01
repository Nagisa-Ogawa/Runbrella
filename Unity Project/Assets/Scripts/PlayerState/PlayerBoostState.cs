﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostState : IState
{
    public void Entry(int ID)
    {
        // ブーストの開始処理
        SceneController.Instance.playerEntityData.playerBoosts[ID].BoostStart();
        // ブーストエフェクト再生
        var player = SceneController.Instance.playerEntityData.players[ID].GetComponent<Player>();
        player.PlayEffect(player.boostEffect);
        player.Rigidbody.velocity = Vector2.zero;
        // 弾消去エリア展開
        SceneController.Instance.playerEntityData.playerBoosts[ID].VanishBulletsArea_ON();
    }

    public void Entry(int ID, RaycastHit2D hit)
    {
    }

    public void Do(int ID)
    {
        // ブーストが終了するかチェック
        if (SceneController.Instance.playerEntityData.playerBoosts[ID].FinishCheck())
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // ショットボタンが押されたら
        if (InputManager.Instance.AttackKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerAttacks[ID].Attack();
        }
       
        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[ID].IsHit == true)
        {
            // 弾に当たった判定をOFFにする。
            SceneController.Instance.playerEntityData.playerAttacks[ID].IsHit = false;
        }
    }

    public void Do_Fix(int ID)
    {
        // 加速処理
        SceneController.Instance.playerEntityData.playerBoosts[ID].Boost();
    }


    public void Exit(int ID)
    {
        // ブーストエフェクト停止
        var player = SceneController.Instance.playerEntityData.players[ID].GetComponent<Player>();
        player.StopEffect(player.boostEffect);
        // 弾消去エリア解消
        SceneController.Instance.playerEntityData.playerBoosts[ID].VanishBulletsArea_OFF();
    }
}

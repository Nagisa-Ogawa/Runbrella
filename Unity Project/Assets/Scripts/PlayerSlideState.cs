﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : IState
{
    public void Entry(int ID)
    {
        // 滑走の開始処理
        SceneController.Instance.playerEntityData.playerSlides[ID].StartSlide();       
    }

    public void Do(int ID)
    {
        // 手すりの上にいるかのチェック処理
        SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
        // 手すりから離れたら
        var hit = SceneController.Instance.playerEntityData.playerSlides[ID].Hit;
        if (hit == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // 地面についたら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);

        }

        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.players[ID].IsHitBullet == true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, ID);
        }


    }

    public void Do_Fix(int ID)
    {
        // 滑走処理
        SceneController.Instance.playerEntityData.playerSlides[ID].Slide();        
        
        
    }

    public void Exit(int ID)
    {
        // 滑走の終了処理
        SceneController.Instance.playerEntityData.playerSlides[ID].EndSlide();
    }    
}

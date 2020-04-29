﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : IState
{
    public void Entry(CONTROLLER_NO controllerNo)
    {
        // 滑走の開始処理
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].StartSlide();       
    }

    public void Do(CONTROLLER_NO controllerNo)
    {
        // 手すりの上にいるかのチェック処理
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].SlideCheck();
        

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(controllerNo))
        {
            var rigidBody = SceneController.Instance.playerEntityData.players[controllerNo].Rigidbody;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);
        }
        
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(controllerNo))
        {
            var rigidBody = SceneController.Instance.playerEntityData.players[controllerNo].Rigidbody;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[controllerNo].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);

        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[controllerNo].IsHit == true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, controllerNo);
        }


    }

    public void Do_Fix(CONTROLLER_NO controllerNo)
    {
        // 手すりから離れたら
        var rayHit = SceneController.Instance.playerEntityData.playerSlides[controllerNo].RayHit;
        var colliderHit = SceneController.Instance.playerEntityData.playerSlides[controllerNo].IsColliderHit;
        if (colliderHit == false && rayHit == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAfterSlideState, controllerNo);
        }
        // 滑走処理
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].Slide();
        // 接地判定
        SceneController.Instance.playerEntityData.playerHitCheckers[controllerNo].GroundCheckSlider();
        //// 地面についたら
        if (SceneController.Instance.playerEntityData.players[controllerNo].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, controllerNo);

        }
    }

    public void Exit(CONTROLLER_NO controllerNo)
    {
        // 滑走の終了処理
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].EndSlide();
    }    
}

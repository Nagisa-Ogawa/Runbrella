﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{   
    public void Entry(CONTROLLER_NO controllerNo)
    {        
    }
   

    public void Do(CONTROLLER_NO controllerNo)
    {
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(controllerNo))
        {
            //Debug.Log(SceneManager.Instance.playerEntityData.players[ID].IsGround);
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[controllerNo].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);

        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(controllerNo))
        {
            // 手すりヒット判定
            SceneController.Instance.playerEntityData.playerSlides[controllerNo].RayTimerStart(0.1f, controllerNo);
        }
        else
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[controllerNo].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[controllerNo].RayHit;
            var colliderHit = SceneController.Instance.playerEntityData.playerSlides[controllerNo].IsColliderHit;
            // 手すりにヒットしていたら
            if (colliderHit == true || raycastHit == true)
            {
                // エフェクトをONにする
                SceneController.Instance.playerEntityData.playerSlides[controllerNo].EffectOn();
            }
            else
            {
                // エフェクトを少し加えるかチェックする
                SceneController.Instance.playerEntityData.playerSlides[controllerNo].SliderCheckSoon();
            }

            SceneController.Instance.playerEntityData.playerAerial[controllerNo].UpdraftCheck();
        }

        // 地面から落ちたら
        if (SceneController.Instance.playerEntityData.players[controllerNo].IsGround == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);
        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[controllerNo].IsHit==true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, controllerNo);
        }

        // アタックボタンが押されたら
        if (InputManager.Instance.AttackKeyIn(controllerNo))
        {
            SceneController.Instance.playerEntityData.playerAttacks[controllerNo].Attack();
        }

        if (InputManager.Instance.BoostKeyHold(controllerNo))
        {
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].Charge();
        }
        else if(InputManager.Instance.BoostKeyOut(controllerNo))
        {
            if (SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostCheck())
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, controllerNo);
            }
        }

    }

    public void Do_Fix(CONTROLLER_NO controllerNo)
    {
        SceneController.Instance.playerEntityData.playerRuns[controllerNo].Run();
    }

    public void Exit(CONTROLLER_NO controllerNo)
    {
        // エフェクトをOFFにする
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].EffectOff();
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].EffectOff();
    }
}

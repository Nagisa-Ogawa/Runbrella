﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : IState
{

    float maxSpeedY = 10;
    float maxSpeedX = 10;


    public void Entry(int ID)
    {
        // デバッグ用色変更
        var rigidBody = SceneController.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>();
        if(rigidBody.velocity.y > maxSpeedY)
        {
            SceneController.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().velocity
                = new Vector2(rigidBody.velocity.x, maxSpeedY);
        }
        if (rigidBody.velocity.x > maxSpeedX)
        {
            SceneController.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().velocity
                = new Vector2(maxSpeedX, rigidBody.velocity.x);
        }
        var sprite = SceneController.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = Color.cyan;
    }

    public void Entry(int ID, RaycastHit2D hit)
    {
    }

    public void Do(int ID)
    {
        // ジャンプボタンが押されたら
        if (InputManager.Instance.StartGlidingKeyIn(ID))
        {
            // 滑空状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerGlideState, ID);
        }
        // 着地したら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);

        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].Hit;            

            // 手すりにヒットしていたら
            if (raycastHit == true)
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerSlideState, ID);

            }
        }

        // ショットボタンが押されたら
        if(InputManager.Instance.ShotKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerShots[ID].
                Shot(SceneController.Instance.playerObjects[ID].transform.position);
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
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        SceneController.Instance.playerEntityData.playerSpeedChecks[ID].SpeedCheck();
    }


    public void Exit(int ID)
    {
        
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    public float downSpeed = 2;
    [SerializeField]
    float rainDownSpeed = 4;
    public float defaultSpeed = 6;
    [SerializeField]
    float RainSpeed = 8;
    Player player;
    Rigidbody2D rigidbody2d;
    private PlayerAerial playerAerial;

    private void Start()
    {
        // rigidbodyをセット
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAerial = GetComponent<PlayerAerial>();
    }


    private void Update()
    {
        // 雨が降っているときはスピードを上げる
        if(player.IsRain)
        {
            // プレイヤーがダウンしているなら
            if (player.state == PlayerStateManager.Instance.playerDownState)
            {
                if(player.BaseSpeed==rainDownSpeed)
                {
                    return;
                }
                SetSpeed(rainDownSpeed);
            }
            else
            {
                if (player.BaseSpeed == RainSpeed)
                {
                    return;
                }
                SetSpeed(RainSpeed);
            }
        }
        // 雨が降っていないならスピードを戻す
        else
        {
            // プレイヤーがダウンしているなら
            if(player.state==PlayerStateManager.Instance.playerDownState)
            {
                if(player.BaseSpeed==downSpeed)
                {
                    return;
                }
                SetSpeed(downSpeed);
            }
            else
            {
                if (player.BaseSpeed == defaultSpeed)
                {
                    return;
                }
                SetSpeed(defaultSpeed);
            }
        }
    }


    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void Run()
    {       
        // 速度の制限処理
        var velocity = rigidbody2d.velocity;
        velocity.x -= decaySpeed;
        if (velocity.x < player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed;
        }
        rigidbody2d.velocity = velocity;
    }


    /// <summary>
    /// プレイヤーの移動速度をセットする関数
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        player.BaseSpeed = speed;
    }

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField]
    float addVelocityX = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // 加速
            {
                //var player = collision.gameObject.GetComponent<Player>();
                //var addVelocity = new Vector2(addVelocityX, 0);
                //player.Rigidbody.velocity += addVelocity;
            }

            // 弾数増加
            {
                var playerAttack = collision.gameObject.GetComponent<PlayerAttack>();
                playerAttack.AddBulletCount(1);
            }
            
        }
    }
    
}

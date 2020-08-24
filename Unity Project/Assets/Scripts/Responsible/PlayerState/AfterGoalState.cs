using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGoalState : MonoBehaviour,IState
{

    private PlayerMove playerMove;
    private PlayerAerial playerAerial;
    private Rigidbody2D rigidbody2;

    // Start is called before the first frame update
    void Start() 
    {
        playerMove = GetComponent<PlayerMove>();
        playerAerial = GetComponent<PlayerAerial>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    void IState.Start()
    {
        // 角度を初期化
        gameObject.transform.localRotation = Quaternion.identity;
        // 重力を変更
        rigidbody2.gravityScale = playerAerial.aerialGravityScale;
        
    }

    void IState.Update()
    {
    }

    void IState.FixedUpdate()
    {
        // 減速処理
        playerMove.MinusAcceleration();
        playerMove.SpeedChange();
    }

    void IState.Exit()
    {
    }

}

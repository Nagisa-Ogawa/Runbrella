using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// 開始処理
    /// </summary>
    void Start();
       
    /// <summary>
    /// UpDateで呼ばれる処理
    /// </summary>
    void Update();

    /// <summary>
    /// FixUpdateで呼ばれる処理
    /// </summary>
    void FixedUpdate();

    /// <summary>
    /// 終了処理
    /// </summary>
    void Exit();

}

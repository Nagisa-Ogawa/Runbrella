using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIExitState : MonoBehaviour, IState
{

    NewsUIExit newsUIExit;

    private void Awake()
    {
        newsUIExit = GetComponent<NewsUIExit>();
    }

    void IState.Start()
    {
        // Entry処理
        newsUIExit.StartExit();
    }

    void IState.Update()
    {
        // Do処理
        newsUIExit.OnExit();
    }

    void IState.FixedUpdate()
    {
        
    }

    public void Exit()
    {
        // Exit処理
        newsUIExit.EndExit();
    }

}
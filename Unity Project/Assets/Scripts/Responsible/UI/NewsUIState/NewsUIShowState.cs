using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIShowState : MonoBehaviour,IState
{

    NewsUIShow newsUIShow;

    private void Awake()
    {
        newsUIShow = GetComponent<NewsUIShow>();
    }

    void IState.Start()
    {
        // Entry処理
        newsUIShow.StartShow();
    }

    void IState.Update()
    {
        // Do処理
        newsUIShow.OnShow();
    }

    void IState.FixedUpdate()
    {
        
    }

    public void Exit()
    {
        // Exit処理
        newsUIShow.EndShow();
    }
}

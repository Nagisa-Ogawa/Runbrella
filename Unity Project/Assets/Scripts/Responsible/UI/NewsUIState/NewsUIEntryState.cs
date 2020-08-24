using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIEntryState : MonoBehaviour,IState
{

    NewsUIEntry newsUIEntry;

    private void Awake()
    {
        newsUIEntry = GetComponent<NewsUIEntry>();
    }

    
    void IState.Start()
    {
        // Entry処理
        newsUIEntry.StartEntry();

    }

    void IState.Update()
    {
        // Entryの移動処理
        newsUIEntry.OnEntry();
    }

    void IState.FixedUpdate()
    {
        
    }

    public void Exit()
    {
        newsUIEntry.EndEntry();
    }

}

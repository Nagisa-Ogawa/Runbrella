﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region シングルトン
    // シングルトン
    private static SceneManager instance;
    public static SceneManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    #endregion

    PlayerRunState playerRunState = new PlayerRunState();

    public Dictionary<int, GameObject> Players = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Ready());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ゲーム開始前の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Ready()
    {
        var playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        var player=Instantiate(playerPrefab);
        Players.Add(1, player);
        PlayerStateManager.Instance.ChangeState(playerRunState,1);
        Debug.Log("北尾");
        yield break;
    }


    /// <summary>
    /// ゲーム中の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGame()
    {

        yield break;
    }


    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator End()
    {
        yield break;
    }


    /// <summary>
    /// プレイヤー作成処理
    /// </summary>
    void CreatePlayer()
    {

    }

}

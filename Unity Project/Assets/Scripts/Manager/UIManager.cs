﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region シングルトン
    // シングルトン
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    #endregion

    [SerializeField]
    List<GameObject> countdowns = new List<GameObject>();

    // リザルトアニメーション
    [SerializeField]
    Animator resultAnimator = null;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<countdowns.Count;i++)
        {
            countdowns[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartResult()
    {
        // アニメーション開始
        resultAnimator.SetBool("isStartCutIn", true);
    }

    /// <summary>
    /// ゲームスタート時のカウントダウンをする関数
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCountdown()
    {
        for(int i=0; i<countdowns.Count;i++)
        {
            countdowns[i].SetActive(true);
            yield return new WaitForSeconds(1);
            countdowns[i].SetActive(false);
        }

    }
}
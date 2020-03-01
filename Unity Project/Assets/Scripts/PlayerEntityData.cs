﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityData
{
    // 各プレイヤーのプレイヤーコンポーネント
    public Dictionary<int, Player> players;
    // 各プレイヤーのランコンポーネント
    public Dictionary<int, PlayerRun> playerRuns;
    // 各プレイヤーのジャンプコンポーネント
    public Dictionary<int, PlayerJump> playerJumps;    
    // 各プレイヤーの手すりチェックコンポーネント
    public Dictionary<int, PlayerSlide> playerSlides;
    // 各プレイヤーのショットチェックコンポーネント
    public Dictionary<int, PlayerShot> playerShots;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerCount"></param>
    public PlayerEntityData(int playerCount)
    {
        // クラスの実体作成
        players = new Dictionary<int, Player>();
        playerRuns = new Dictionary<int, PlayerRun>();
        playerJumps = new Dictionary<int, PlayerJump>();
        playerSlides = new Dictionary<int, PlayerSlide>();
        playerShots = new Dictionary<int, PlayerShot>();

        // 各プレイヤーのコンポーネントの実体格納
        for (int ID = 1; ID <= playerCount; ID++)
        {
            var player = SceneManager.Instance.playerObjects[ID].GetComponent<Player>();
            var playerRun = SceneManager.Instance.playerObjects[ID].GetComponent<PlayerRun>();
            var playerJump = SceneManager.Instance.playerObjects[ID].GetComponent<PlayerJump>();
            var playerSlide = SceneManager.Instance.playerObjects[ID].GetComponent<PlayerSlide>();
            var playerShot = SceneManager.Instance.playerObjects[ID].GetComponent<PlayerShot>();

            players.Add(ID, player);
            playerRuns.Add(ID, playerRun);
            playerJumps.Add(ID, playerJump);
            playerSlides.Add(ID, playerSlide);
            playerShots.Add(ID, playerShot);

        }

    }

    
    
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region シングルトン
    // シングルトン
    private static SceneController instance;
    public static SceneController Instance
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

        StartCoroutine(Ready());

    }


    #endregion


    // プレイヤーのGameObjectを格納するディクショナリー
    public Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();
    // 各プレイヤーのコンポーネントの実体が格納されたディクショナリー
    public PlayerEntityData playerEntityData;

    // プレイヤーの人数
    [SerializeField]
    public int playerCount = 0;
    // ゲームがスタートしているかどうか
    public bool isStart;
    // 誰かがゴールしているか
    public bool isGoal;
    [SerializeField]
    AudioClip stageBGM = null;
    public int deadPlayerCount = 0;
    public int goalPlayerCount = 0;
    [SerializeField]
    // ゴールしたプレイヤーの配列
    private List<GameObject> goalRunkOrder = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
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
        CreatePlayer();
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(UIManager.Instance.StartCountdown());

        for (int i = 1; i <= playerCount; i++)
        {
            // Run状態にチェンジ
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, i);
            // プレイヤーが画面外に出たかどうかのコンポーネントを追加
            playerObjects[i].AddComponent<PlayerCheckScreen>();
        }
        AudioManager.Instance.PlayBGM(stageBGM, true, 0.5f);
        // ゲーム開始フラグをtrueにする
        isStart = true;
        StartCoroutine(OnGame());
        yield break;
    }


    /// <summary>
    /// ゲーム中の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGame()
    {
        while(true)
        {
            if(Input.GetButtonDown("player1_Restart") || Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.Application.Quit();
            }

            // 残り一人になったら終了
            //if(CheckSurvivor()==1)
            //{
            //    StartEnd();
            //    yield break;
            //}

            yield return null;
        }
    }


    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator End()
    {
        // リザルトシーンを読み込む
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // ゴールフラグをON
        isGoal = true;
        while (true)
        {
            // すべてのプレイヤーが画面外に行ったかチェック
            if (CheckSurvivor() < 1)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1);
        UIManager.Instance.StartResult();

    }


    /// <summary>
    /// プレイヤー作成処理
    /// </summary>
    void CreatePlayer()
    {
        for (int ID = 1; ID <= playerCount; ID++)
        {
            // プレイヤーを作成
            var playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            var player = Instantiate(playerPrefab);
            // PlayersにプレイヤーのIDとGameObjectを格納
            playerObjects.Add(ID, player);
            // プレイヤーのID設定
            playerObjects[ID].GetComponent<Player>().ID = ID;
            // プレイヤーのIDをプレイヤーの子オブジェクトに渡す
            playerObjects[ID].transform.Find("PlayerInformation").GetComponent<PlayerInformation>().playerID = ID;
            // Stateを初期化
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerIdelState, ID);
        }
        playerEntityData = new PlayerEntityData(playerCount);
    }


    /// <summary>
    /// プレイヤーが何人生きているかを返す関数
    /// </summary>
    int CheckSurvivor()
    {
        int survivor = 0;

        // プレイヤーの生存チェック
        for(int i=1;i<=playerCount;i++)
        {
            if(playerObjects[i].activeInHierarchy)
            {
                survivor++;
            }
        }

        return survivor;
    }


    /// <summary>
    /// エンドコルーチンを開始
    /// </summary>
    public void StartEnd(GameObject playerObject)
    {
        // すべてのコルーチンを停止
        StopAllCoroutines();
        // ゴールしたプレイヤーの状態をRunにチェンジ
        var player = playerObject.GetComponent<Player>();
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, player.ID);
        // 終了処理コルーチンを開始
        StartCoroutine(End());
    }


    /// <summary>
    /// 死んだプレイヤーをリストに格納する関数
    /// </summary>
    public void InsertDeadPlayer(GameObject player)
    {
        // 同じプレイヤーがいるなら格納しない
        for(int i=0;i<goalRunkOrder.Count;i++)
        {
            if(goalRunkOrder[i]==player)
            {
                return;
            }
        }

        goalRunkOrder.Insert(goalRunkOrder.Count - deadPlayerCount, player);
    }


    /// <summary>
    /// ゴールしたプレイヤーをリストに格納する関数
    /// </summary>
    public void InsertGoalPlayer(GameObject player)
    {
        // 同じプレイヤーがいるなら格納しない
        for (int i = 0; i < goalRunkOrder.Count; i++)
        {
            if (goalRunkOrder[i] == player)
            {
                return;
            }
        }

        goalRunkOrder.Insert(goalPlayerCount, player);
    }


}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinResultUI : MonoBehaviour
{
    // プレイヤーのリザルトUIのコイン用UI
    List<List<GameObject>> minCoinUIs = new List<List<GameObject>>();
    // コイン用UIのポジション
    List<List<Vector3>> minCoinUIsPos = new List<List<Vector3>>();
    // リザルトUIのリスト
    List<GameObject> minPlayerResultUIs = new List<GameObject>();
    // ゴールコインのSprite
    [SerializeField]
    Sprite goalCoinSprite = null;
    // minResultUI
    [SerializeField]
    private GameObject minResultUI = null;



    /// <summary>
    /// MinPlayerResultUIを生成する関数
    /// </summary>
    public void CreateMinPlayerResultUI()
    {
        // UIを作成
        var minPlayerResultUIPrefab = Resources.Load<GameObject>("Prefabs/MinPlayerResultUI");
        // 作成するUIのWidth
        float width = minPlayerResultUIPrefab.GetComponent<RectTransform>().sizeDelta.x;
        // minResultUIのWidth
        float minResultUIWidth = minResultUI.GetComponent<RectTransform>().sizeDelta.x;
        // プレイヤーの数によってオフセットを決める
        float offsetX = (minResultUIWidth - (width * GameManager.Instance.playerNumber))
            / (GameManager.Instance.playerNumber + 1f);
        float resultOffsetX = (-minResultUIWidth / 2f) + ((width / 2f) + offsetX);
        // プレイヤーの数だけ作成
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            var minResultUIObj = Instantiate(minPlayerResultUIPrefab);
            // リザルトUIのテキストを設定
            var minResultUIText = minResultUIObj.transform.Find("MinPlayerName/MinPlayerNameText").gameObject.GetComponent<Text>();
            minResultUIText.text = "Player" + (i + 1).ToString();
            // リストに追加
            minPlayerResultUIs.Add(minResultUIObj);
            // minResultUIの子オブジェクトに変更
            minResultUIObj.transform.SetParent(minResultUI.transform);
            // プレイヤーカラーに設定
            minResultUIObj.GetComponent<Image>().color = UIManager.Instance.playerColors[i];
            // PlayerCoinUIを作成
            CreateMinCoinUI(minResultUIObj, i);
            // playerCoinUIを格納するリストを作成
            List<GameObject> minCoinUI = new List<GameObject>();
            // レース数回ループ
            for (int loop = 0; loop < GameManager.Instance.RaceNumber; loop++)
            {
                // コイン用UIをリストに格納
                GameObject coinUIObj =
                    minResultUIObj.transform.Find("MinCoinUI" + i.ToString() + "_" + loop.ToString()).gameObject;
                minCoinUI.Add(coinUIObj);
            }
            minCoinUIs.Add(minCoinUI);
            // 各プレイヤーの勝ち数によってコイン用UIのスプライトを変更
            if (GameManager.Instance.playerWins.Count > 0)
            {
                for (int l = 0; l < GameManager.Instance.playerWins[(PLAYER_NO)i]; l++)
                {
                    minCoinUIs[i][l].GetComponent<Image>().sprite = goalCoinSprite;
                }
            }
            // 座標を変更
            Vector3 resultUIPos = new Vector3(resultOffsetX, minResultUIObj.transform.position.y, 0);
            minResultUIObj.transform.localPosition = resultUIPos;
            // オフセットをずらす
            resultOffsetX += (width + offsetX);
        }
    }


    /// <summary>
    ///　プレイヤーのコインはまる用UIを作成する関数
    /// </summary>
    /// <param name="minPlayerResultObj"></param>
    /// <param name="num"></param>
    private void CreateMinCoinUI(GameObject minPlayerResultUI, int num)
    {
        // リソースからロード
        var minCoinUIPrefab = Resources.Load<GameObject>("Prefabs/MinCoinUI");
        // 作成するUIのwidth
        float minCoinUIWidth = minCoinUIPrefab.GetComponent<RectTransform>().sizeDelta.x;
        // リザルトUIのwidth
        float minPlayerResultUIWidth = minPlayerResultUI.GetComponent<RectTransform>().sizeDelta.x;
        // プレイヤーの数によってPlayerCoinUIのオフセットを決める
        float OffsetX = (minPlayerResultUIWidth - minCoinUIWidth * GameManager.Instance.RaceNumber)
            / (GameManager.Instance.RaceNumber + 1);
        float playerCoinOffset = (-minPlayerResultUIWidth / 2f) + (minCoinUIWidth / 2f) + OffsetX;
        // プレイヤーの数だけ作成
        for (int i = 0; i < GameManager.Instance.RaceNumber; i++)
        {
            // 生成
            var minCoinObj = Instantiate(minCoinUIPrefab);
            // resultUIの子オブジェクトにする
            minCoinObj.transform.parent = minPlayerResultUI.transform;
            // 座標を変更
            Vector3 pos = new Vector3(playerCoinOffset, minCoinObj.transform.position.y, 0);
            minCoinObj.transform.localPosition = pos;
            // オフセットを変更
            playerCoinOffset += (minCoinUIWidth + OffsetX);
            // 名前を変更
            minCoinObj.name = "Min" +
                "CoinUI" + num.ToString() + "_" + i.ToString();
        }
    }


    /// <summary>
    /// MinPlayerResultUIを非表示にする関数
    /// </summary>
    public void HideMinResultUI()
    {
        for(int i=0;i<minPlayerResultUIs.Count;i++)
        {
            minPlayerResultUIs[i].SetActive(false);
        }
    }

}
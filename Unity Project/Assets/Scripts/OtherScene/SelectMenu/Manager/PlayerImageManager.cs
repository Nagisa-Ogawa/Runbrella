﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageManager : MonoBehaviour
    {
        // プレイヤーの画像のコンポーネントのリスト
        List<PlayerImage> playerImages = new List<PlayerImage>();
        // キャラ選択が終了したプレイヤーの画像のコンポーネントのディクショナリー
        Dictionary<int, PlayerImage> entryPlayerImages = new Dictionary<int, PlayerImage>();
        // キャラ選択を管理するマネージャー
        private SelectCharacterManager selectCharacterManager;

        #region ステート変数
        private PlayerImageIdleState idleState = new PlayerImageIdleState();
        private PlayerImageRunState runState = new PlayerImageRunState();
        private PlayerImageBoostState boostState = new PlayerImageBoostState();
        private PlayerImageGoalState goalState = new PlayerImageGoalState();
        // get
        public PlayerImageGoalState GoalState { get { return goalState; } }
        public PlayerImageBoostState BoostState { get { return boostState; } }
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            selectCharacterManager = GetComponent<SelectCharacterManager>();
            // プレイヤーの画像のプレファブを配列にセット
            StartCoroutine(SetPlayerImage());
        }

        /// <summary>
        /// プレイヤーの画像のプレファブを配列にセットするメソッド
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetPlayerImage()
        {
            // オブジェクトの配列の取得
            var playerImageObjects = GameObject.FindGameObjectsWithTag("PlayerImage");
            // 処理順を変えるために1フレーム待つ
            yield return null;
            foreach (var playerImageObject in playerImageObjects)
            {
                // コンポーネントを取得
                var playerImage = playerImageObject.GetComponent<PlayerImage>();
                // ステートの変更
                playerImage.ChangeState(idleState);
                // リストに追加
                playerImages.Add(playerImage);
            }
        }

        /// <summary>
        /// 参加処理
        /// </summary>
        /// <param name="ID"></param>
        public void PlayerImageEntry(int ID)
        {
            var playerImage = GetPlayerImage();
            var selectCharaNumber = selectCharacterManager.SelectCharacters[ID].SelectCharacterNumber;
            var animatorController = Resources.Load<RuntimeAnimatorController>(string.Format("PlayerImageAnimator/{0}",
                SceneController.Instance._characterMessages[selectCharaNumber].charaType.ToString()));
            playerImage._animator.runtimeAnimatorController = animatorController;
            playerImage.ChangeState(runState);
            entryPlayerImages.Add(ID, playerImage);
        }

        /// <summary>
        /// 待機状態のプレイヤーのイメージのコンポーネントを取得
        /// </summary>
        /// <returns></returns>
        private PlayerImage GetPlayerImage()
        {
            foreach(var playerImage in playerImages)
            {
                if(playerImage.State != idleState)
                {
                    continue;
                }

                return playerImage;
            }

            Debug.Log("PlayerImage" + null);
            return null;
        }

        /// <summary>
        /// 参加状態のプレイヤーのキャンセル
        /// </summary>
        /// <param name="ID"></param>
        public void PlayerImageCansel(int ID)
        {
            entryPlayerImages[ID].ChangeState(idleState);
            entryPlayerImages.Remove(ID);
        }

        /// <summary>
        /// 画面内のプレイヤーの全画像をブーストに変更
        /// </summary>
        public void AllPlayerImageBoost()
        {
            // ディクショナリーから値だけを取り出す
            foreach(var playerImage in entryPlayerImages.Values)
            {
                // 取り出したコンポーネントのステート
                var state = playerImage.State;
                // 画面内にいるなら
                if(state != goalState && state != idleState)
                {
                    // ステートをブーストに変更
                    playerImage.ChangeState(boostState);
                }
            }
                   
        }

        /// <summary>
        /// 画面内にプレイヤーがいるかチェック
        /// </summary>
        /// <returns>画面内にプレイヤーがいるかいないか</returns>
        public bool FinishCheck()
        {
            foreach(var playerImage in entryPlayerImages.Values)
            {
                if(playerImage.State != goalState)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
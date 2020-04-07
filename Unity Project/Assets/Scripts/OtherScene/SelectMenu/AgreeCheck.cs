﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class AgreeCheck : MonoBehaviour
    {
        // 同意する/同意しない を表すオブジェクトの配列変数
        private GameObject[] isAgreeObjects = new GameObject[(int)SelectPlayCount.IsAgree.Disagree + 1];

        private ScalingAnimation scalingAnimation;

        // Start is called before the first frame update
        void Start()
        {
            scalingAnimation = GetComponent<ScalingAnimation>();

            // 配列にセット
            for (var i = SelectPlayCount.IsAgree.Agree; i <= SelectPlayCount.IsAgree.Disagree; i++)
            {
                // 子オブジェクトから文字列探索
                isAgreeObjects[(int)i] = transform.Find(i.ToString()).gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for(var i = (int)SelectPlayCount.IsAgree.Agree; i <= (int) SelectPlayCount.IsAgree.Disagree; i++)
            {
                // 選択中なら
                if(i == SceneController.Instance._selectPlayCount._isAgree)
                {
                    // ゲームオブジェクトをセット
                    scalingAnimation.SetScalingObject(isAgreeObjects[i]);
                }
                // 選択中でないのなら
                else
                {
                    // 元の大きさに変更する
                    isAgreeObjects[i].transform.localScale = Vector3.one;
                } // else
            } // for
        } // update

    } // class
} // namespace


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// スコアなどのゲーム管理
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public const int CharacterMax = 8;

        [Tooltip("爆発エフェクト"), SerializeField]
        GameObject explosionPrefab = null;

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// ダメージ処理から呼び出して処理
        /// </summary>
        /// <param name="loseObj">食らったプレイヤー</param>
        public void Hit(GameObject loseObj)
        {
            // 爆発(あとでNetworkServer.Spawnする)
            Instantiate(explosionPrefab, 
                loseObj.transform.position,
                Quaternion.identity);
        }

        /// <summary>
        /// ゲームから退出します。
        /// </summary>
        public void GameExit()
        {
            IGameExit exit = GetComponentInChildren<IGameExit>();
            if (exit != null)
            {
                exit.Exit();
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("GameManagerの子供にIGameExitを実装してください。");
            }
#endif

        }
    }
}
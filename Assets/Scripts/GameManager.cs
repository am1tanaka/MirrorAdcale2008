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

        /// <summary>
        /// プレイヤーの人数
        /// </summary>
        public static int PlayerCount { get; private set; } = 0;

        void Awake()
        {
            Instance = this;
            PlayerCount = 0;
        }

        /// <summary>
        /// プレイヤーを登録します。
        /// ネットワーク対応したらNetworkManagerのクライアント数で処理できるので不要になります。
        /// </summary>
        public void EntryPlayer()
        {
            if (PlayerCount >= CharacterMax)
            {
#if UNITY_EDITOR
                Debug.Log("プレイヤーの登録数をオーバーしました。");
#endif
                return;
            }

            PlayerCount++;
        }

        /// <summary>
        /// プレイヤーを減らす。
        /// ネットワーク対応したらNetworkManagerのクライアント数で処理できるので不要になります。
        /// </summary>
        public void RemovePlayer()
        {
            PlayerCount--;
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
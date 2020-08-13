using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    public class StandaloneManager : MonoBehaviour, IGameExit
    {
        [Tooltip("プレイヤープレハブ"), SerializeField]
        PlayerAction playerPrefab = null;
        [Tooltip("COMプレハブ"), SerializeField]
        PlayerAction comPrefab = null;

        CanvasGroup canvasGroup = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Exit();
        }

        void FixedUpdate()
        {
            // 敵が不足していたら追加
            for (int i = GameManager.PlayerCount; i < GameManager.CharacterMax - 1; i++)
            {
                Material mat = PlayerMaterialManager.Instance.GetMaterial();
                if (mat != null)
                {
                    Instantiate(comPrefab).Init(
                        StartPositions.Instance.GetPoint(),
                        mat
                        );
                }
            }
        }

        public void EntryPlayer()
        {
            // プレイヤー生成
            Material mat = PlayerMaterialManager.Instance.GetMaterial();
            if (mat != null)
            {
                Instantiate(playerPrefab).Init(
                    StartPositions.Instance.GetPoint(),
                    mat,
                    true
                );
            }

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// ゲームから退出
        /// </summary>
        public void Exit()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
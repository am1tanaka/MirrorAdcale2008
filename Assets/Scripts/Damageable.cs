using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        [Tooltip("登場直後の無敵時間"), SerializeField]
        float muteki = 2;

        /// <summary>
        /// 無敵時のα
        /// </summary>
        const float MutekiAlpha = 0.35f;

        PlayerAction playerAction = null;
        IMiss currentMiss = null;
        bool isMuteki = false;
        Collider currentCollider = null;

        private void Awake()
        {
            playerAction = GetComponent<PlayerAction>();
            currentMiss = GetComponent<IMiss>();
            currentCollider = GetComponent<Collider>();
            currentCollider.enabled = false;
            isMuteki = false;
        }

        /// <summary>
        /// 無敵状態を設定します。
        /// </summary>
        /// <param name="flag">true=無敵 / false=無敵解除</param>
        void SetMuteki(bool flag)
        {
            isMuteki = flag;
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            Color c = meshRenderer.material.color;
            c.a = flag ? MutekiAlpha : 1f;
            meshRenderer.material.color = c;
            currentCollider.enabled = !flag;
        }

        private void Update()
        {
            // 無敵開始チェック
            if ((Time.time-playerAction.StartTime) < muteki)
            {
                if (!isMuteki)
                {
                    SetMuteki(true);
                }
            }
            else if (isMuteki)
            {
                SetMuteki(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // ダメージ判定
            if (other.CompareTag("Shot"))
            {
                Shot sh = other.GetComponent<Shot>();
                if (sh.playerId != playerAction.PlayerID)
                {
                    // 自分の弾以外の時、死亡
                    Damage(playerAction.CurrentMaterial.color);

                    // ミス処理呼び出し
                    currentMiss.Miss();
                }

                // 弾を消す(自分の弾も消す)
                Destroy(other.gameObject);
            }
        }

        void Damage(Color c)
        {
            PlayerMaterialManager.Instance.UnuseMaterial(c);
            GameManager.Instance.Hit(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// 弾の制御
    /// </summary>
    public class Shot : MonoBehaviour
    {
        [Tooltip("ショット速度"), SerializeField]
        float speed = 10f;
        [Tooltip("ショット寿命秒数"), SerializeField]
        float lifeTime = 3f;

        /// <summary>
        /// 射出したプレイヤーのインスタンス
        /// </summary>
        public PlayerAction CurrentPlayerAction { get; private set; } = null;

        [HideInInspector]
        public uint playerId;

        Color currentColor;

        /// <summary>
        /// 指定の速度でショット
        /// </summary>
        /// <param name="act">プレイヤー</param>
        /// <param name="pos">座標</param>
        /// <param name="dir">飛んで行く向きベクトル</param>
        public void Shoot(PlayerAction act, Vector3 pos, Vector3 dir)
        {
            CurrentPlayerAction = act;

            // ショット
            transform.position = pos;
            transform.forward = dir;
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
            currentColor = act.CurrentMaterial.color;
            playerId = act.PlayerID;
            Invoke(nameof(DestroyMe), lifeTime);

            var rend = GetComponentInChildren<MeshRenderer>();
            rend.material = act.CurrentMaterial;
        }

        void DestroyMe()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (CurrentPlayerAction != null)
            {
                CurrentPlayerAction.ShotRecovery();
            }
        }
    }
}
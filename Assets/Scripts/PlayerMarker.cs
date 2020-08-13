using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// プレイヤー登場時にプレイヤーの位置を示すためのマーカー
    /// </summary>
    public class PlayerMarker : MonoBehaviour
    {
        [Tooltip("表示秒数"), SerializeField]
        float lifeTime = 1f;
        [Tooltip("近寄り方アニメ"), SerializeField]
        AnimationCurve distanceRate = new AnimationCurve();
        [Tooltip("ターゲットからのオフセット"), SerializeField]
        Vector3 offset = new Vector3(0, -0.15f, 0);
        [Tooltip("ターゲットからの最短距離"), SerializeField]
        float minDistance = 1f;

        /// <summary>
        /// 位置
        /// </summary>
        public enum Position
        {
            Bottom, // 下から。xをプレイヤーと一致
            Right,  // 右から。yをプレイヤーと一致
            Top,    // 上から。xをプレイヤーと一致
            Left    // 左から。yをプレイヤーと一致
        }

        Transform target = null;
        Position fromPosition;
        float startTime = 0;

        /// <summary>
        /// 移動元とターゲットを設定して、アニメ開始
        /// </summary>
        /// <param name="tg">目的のプレイヤーのTransform</param>
        /// <param name="from">画面のどのサイドから出現させるかをPositionで指定</param>
        public void SetParams(Transform tg, Position from)
        {
            startTime = Time.time;
            target = tg;
            fromPosition = from;
            transform.rotation = Quaternion.Euler(0, 0, ((int)from) * 90);
            Update();
        }

        private void Update()
        {
            float keika = Time.time - startTime;
            if (keika > lifeTime)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 from = Vector3.zero;
            Vector3 to = Vector3.zero;
            switch(fromPosition)
            {
                // 下から
                case Position.Bottom:
                    from.Set(target.position.x, Wrap.LoopRect.min.y, 0);
                    to.Set(target.position.x, target.position.y - minDistance, 0);
                    break;

                // 右から
                case Position.Right:
                    from.Set(Wrap.LoopRect.max.x, target.position.y, 0);
                    to.Set(target.position.x + minDistance, target.position.y, 0);
                    break;

                // 上から
                case Position.Top:
                    from.Set(target.position.x, Wrap.LoopRect.max.y, 0);
                    to.Set(target.position.x, target.position.y + minDistance, 0);
                    break;

                // 左から
                case Position.Left:
                    from.Set(Wrap.LoopRect.min.x, target.position.y, 0);
                    to.Set(target.position.x - minDistance, target.position.y, 0);
                    break;
            }

            from += offset;
            to += offset;

            float t = keika / lifeTime;
            transform.position = Vector3.Lerp(from, to, 1f-distanceRate.Evaluate(t)); ;
        }
    }
}
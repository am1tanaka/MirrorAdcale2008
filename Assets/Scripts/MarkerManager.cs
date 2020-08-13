using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    public class MarkerManager : MonoBehaviour
    {
        public static MarkerManager Instance { get; private set; }

        [Tooltip("マーカーオブジェクト"), SerializeField]
        PlayerMarker markerPrefab = null;

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// マーカーを開始します。
        /// </summary>
        /// <param name="pl">プレイヤーのTransformを受け取ります。</param>
        public void Play(Transform pl)
        {
            for (int i=0;i<4;i++)
            {
                Instantiate(markerPrefab).SetParams(pl, (PlayerMarker.Position)i);
            }
        }
    }
}
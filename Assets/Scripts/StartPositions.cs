using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// スタート位置を返します。
    /// </summary>
    public class StartPositions : MonoBehaviour
    {
        public static StartPositions Instance { get; private set; }

        Vector3[] startPositions = null;
        int counter = 0;

        private void Awake()
        {
            Instance = this;

            startPositions = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                startPositions[i] = transform.GetChild(i).transform.position;
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 座標を1つずつ順番に返します。
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPoint()
        {
            Vector3 p = startPositions[counter];
            counter = (counter + 1) % startPositions.Length;
            return p;
        }
    }
}
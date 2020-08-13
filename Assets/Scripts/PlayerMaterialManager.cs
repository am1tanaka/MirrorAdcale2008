using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AM1.MirrorBlog
{
    public class PlayerMaterialManager : MonoBehaviour
    {
        public static PlayerMaterialManager Instance { get; private set; }

        /// <summary>
        /// 使用中のマテリアル数
        /// </summary>
        public static int UseCount { get; private set; } = 0;

        [Tooltip("マテリアル"), SerializeField]
        Material[] materials = null;

        bool[] isUsing = null;

        private void Start()
        {
            Instance = this;
            isUsing = new bool[materials.Length];
            for (int i=0;i<isUsing.Length;i++)
            {
                isUsing[i] = false;
            }
            UseCount = 0;
        }

        /// <summary>
        /// 指定の色を持ったマテリアルを返します。
        /// </summary>
        /// <param name="c">探す色</param>
        /// <returns>見つけたマテリアル。なければ最初のものを返します。</returns>
        public Material GetMaterial(Color c)
        {
            for (int i=0;i<materials.Length;i++)
            {
                if (materials[i].color == c)
                {
                    return materials[i];
                }
            }

            return materials[0];
        }

        /// <summary>
        /// 未使用のマテリアルを返します。
        /// </summary>
        /// <returns>未使用のマテリアル。なければnull</returns>
        public Material GetMaterial()
        {
            for (int i=0;i<isUsing.Length;i++)
            {
                if (!isUsing[i])
                {
                    isUsing[i] = true;
                    UseCount++;
                    return materials[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 指定のマテリアルを未使用に戻します。
        /// </summary>
        /// <param name="material">未使用にしたいマテリアル</param>
        public void UnuseMaterial(Material mat)
        {
            for (int i=0;i<materials.Length;i++)
            {
                if (materials[i] == mat)
                {
                    isUsing[i] = false;
                    UseCount--;
                    return;
                }
            }
        }

        /// <summary>
        /// 指定の色のマテリアルを未使用に戻します。
        /// </summary>
        /// <param name="c">未使用にしたいマテリアルの色</param>
        public void UnuseMaterial(Color c)
        {
            UnuseMaterial(GetMaterial(c));
        }
    }
}
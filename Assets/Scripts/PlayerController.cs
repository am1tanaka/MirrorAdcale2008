using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// プレイヤーを操作する
    /// </summary>
    [RequireComponent(typeof(PlayerAction))]
    public class PlayerController : MonoBehaviour, IMiss
    {
        PlayerAction action = null;
        Vector3 move = Vector3.zero;
        Camera currentCamera = null;
        Camera CurrentCamera
        {
            get
            {
                if (currentCamera == null)
                {
                    currentCamera = Camera.main;
                }
                return currentCamera;
            }
        }
        int floorLayer = 0;

        private void Awake()
        {
            action = GetComponent<PlayerAction>();
            floorLayer = LayerMask.GetMask("Floor");
        }

        private void Update()
        {
            // 操作
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");

            // 向き
            Vector3 dir = action.Forward;
            Ray ray = CurrentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, floorLayer))
            {
                dir = hit.point - transform.position;
                dir.z = 0f;
            }
            action.Move(move, dir);

            // ショット
            if (Input.GetButtonDown("Fire1"))
            {
                action.Shot();
            }
        }

        /// <summary>
        /// ミスしたら、自分を破壊して待機モードへ移行します。
        /// </summary>
        public void Miss()
        {
            Destroy(gameObject);
            GameManager.Instance.GameExit();
        }
    }
}
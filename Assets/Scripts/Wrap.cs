using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// 画面のループ処理
    /// </summary>
    public class Wrap : MonoBehaviour
    {
        public static Rect LoopRect { get; private set; }
        static bool inited = false;
        Rigidbody rb = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (inited) return;

            var cam = Camera.main;

            int floorLayer = LayerMask.GetMask("Floor");

            var min = new Vector3(0, 0, 0);
            RaycastHit hit;
            Physics.Raycast(cam.ViewportPointToRay(min), out hit, float.PositiveInfinity, floorLayer);
            Rect r = new Rect();
            r.min = new Vector2(hit.point.x, hit.point.y);

            var max = new Vector3(1, 1, 0);
            Physics.Raycast(cam.ViewportPointToRay(max), out hit, float.PositiveInfinity, floorLayer);
            r.max = new Vector2(hit.point.x, hit.point.y);
            LoopRect = r;
            inited = true;
        }

        private void FixedUpdate()
        {
            Vector3 pos = transform.position;

            // ループチェック
            if ((rb.velocity.x < 0) && (transform.position.x < LoopRect.min.x))
            {
                pos.x = LoopRect.max.x;
            }
            else if ((rb.velocity.x > 0) && (transform.position.x > LoopRect.max.x))
            {
                pos.x = LoopRect.min.x;
            }

            if ((rb.velocity.y < 0) && (transform.position.y < LoopRect.min.y))
            {
                pos.y = LoopRect.max.y;
            }
            else if ((rb.velocity.y > 0) && (transform.position.y > LoopRect.max.y))
            {
                pos.y = LoopRect.min.y;
            }

            transform.position = pos;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// プレイヤーのAI。とりあえずランダムで
    /// </summary>
    public class ComPlayer : MonoBehaviour, IMiss
    {
        [Tooltip("ショット最短間隔秒数"), SerializeField]
        float shotMin = 0.2f;
        [Tooltip("ショット最長間隔秒数"), SerializeField]
        float shotMax = 2f;
        [Tooltip("操作の維持の最小秒数"), SerializeField]
        float moveMin = 0.2f;
        [Tooltip("操作の維持の最大秒数"), SerializeField]
        float moveMax = 1f;

        PlayerAction playerAction = null;
        Vector3 move = Vector3.zero;
        Vector3 dir = Vector3.up;

        void Awake()
        {
            playerAction = GetComponent<PlayerAction>();
            Invoke(nameof(Shot), Random.Range(shotMin, shotMax));
            dir = transform.up;
            Move();
        }

        void Update()
        {
            playerAction.Move(move, dir);
        }

        void Shot()
        {
            playerAction.Shot();
            Invoke(nameof(Shot), Random.Range(shotMin, shotMax));
        }

        void Move()
        {
            move.Set(Random.Range(0, 3) - 1, Random.Range(0, 3) - 1, 0f);
            float rad = Random.Range(0, Mathf.PI * 2f);
            dir.Set(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            Invoke(nameof(Move), Random.Range(moveMin, moveMax));
        }

        public void Miss()
        {
            Destroy(gameObject);
        }
    }
}
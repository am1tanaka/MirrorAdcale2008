using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// プレイヤーの移動やショットの動作を提供するクラス
    /// </summary>
    public class PlayerAction : MonoBehaviour
    {
        [Header("移動")]
        [Tooltip("旋回速度"), SerializeField]
        float angularSpeed = 90f;
        [Tooltip("加減速"), SerializeField]
        float acceleration = 10f;
        [Tooltip("ショットの反動"), SerializeField]
        float shotBack = 1f;

        [Header("ショット")]
        [Tooltip("ショットオブジェクト"), SerializeField]
        Shot shot = null;
        [Tooltip("射出口"), SerializeField]
        Transform shotPosition = null;

        [Tooltip("連射インターバル"), SerializeField]
        float rapidSpan = 0.04f;
        [Tooltip("ショット上限"), SerializeField]
        int shotMax = 4;

        [Header("デバッグ")]
        [Tooltip("起動時に自動的に初期化をさせる時、true"), SerializeField]
        bool playOnAwake = false;

        /// <summary>
        /// スタンドアロンでプレイヤーを識別する時に使うカウンター
        /// </summary>
        static uint counter = 0;

        /// <summary>
        /// プレイヤーの識別番号
        /// </summary>
        public uint PlayerID { get; private set;}

        /// <summary>
        /// 射出座標を返します
        /// </summary>
        public Vector3 ShotPosition { get { return shotPosition.position; } }

        /// <summary>
        /// 前方を返します
        /// </summary>
        public Vector3 Forward { get { return transform.up; } }

        public Material CurrentMaterial { get; private set; } = null;

        /// <summary>
        /// 操作しているプレイヤーの時、trueを返します。
        /// </summary>
        public bool IsLocalPlayer { get; private set; }

        /// <summary>
        /// 登場した時間
        /// </summary>
        public float StartTime { get; private set; }

        Rigidbody rb = null;
        int shotCount = 0;
        float shotTime = 0f;
        bool started = false;
        /// <summary>
        /// 向き
        /// </summary>
        Vector3 toDir = Vector3.up;

        private void Start()
        {
            GameManager.Instance.EntryPlayer(); // ネット対応時に削除

            // デバッグ機能。起動と同時に自動開始する
            if (playOnAwake)
            {
                Init(transform.position, GetComponentInChildren<MeshRenderer>().material, true);
            }
        }

        private void FixedUpdate()
        {
            if (!started) return;

            float toAngle = Vector3.SignedAngle(transform.up, toDir, Vector3.forward);
            Vector3 av = rb.angularVelocity;
            float tickAngle = angularSpeed * Time.fixedDeltaTime;
            // 通り過ぎるなら方向確定
            if (Mathf.Abs(toAngle) <= tickAngle)
            {
                transform.up = toDir;
                av.Set(0, 0, 0);
            }
            else
            {
                // 目的方向へ展開
                float angle = Mathf.Min(Mathf.Abs(toAngle), tickAngle);
                av.z = angle * Mathf.Sign(toAngle) / Time.fixedDeltaTime;
            }
            rb.angularVelocity = av;
        }

        /// <summary>
        /// 入力を引数に設定して、移動指示を与えます。
        /// </summary>
        /// <param name="axis">x=左右千回 / y=加減速</param>
        /// <param name="dir">向きたい方向ベクトル</param>
        public void Move(Vector3 axis, Vector3 dir)
        {
            if (!started) return;

            // 旋回
            toDir = dir.normalized;

            // 加減速
            Vector3 v = rb.velocity;
            v += axis.normalized * acceleration * Time.deltaTime;
            rb.velocity = v;
        }

        /// <summary>
        /// 弾を撃ちます。
        /// </summary>
        public void Shot()
        {
            if (!started) return;

            // ショットキャンセルチェック
            if ((shotCount >= shotMax) || (Time.time - shotTime < rapidSpan)) return;

            // ショット情報記録
            shotCount++;
            shotTime = Time.time;

            // ショット
            Instantiate<Shot>(shot).Shoot(this, ShotPosition, Forward);

            // 速度
            Vector3 vel = rb.velocity;
            vel -= Forward * shotBack;
            rb.velocity = vel;
        }

        /// <summary>
        /// ショットを回収
        /// </summary>
        public void ShotRecovery()
        {
            shotCount--;
        }

        /// <summary>
        /// 座標とマテリアルを設定して、動作を開始します。
        /// </summary>
        /// <param name="pos">開始座標</param>
        /// <param name="mat">マテリアル</param>
        /// <param name="isLocal">自分のプレイヤーの時、true。デフォルトはfalse</param>
        public void Init(Vector3 pos, Material mat, bool isLocal=false)
        {
            transform.position = pos;
            CurrentMaterial = mat;
            GetComponentInChildren<MeshRenderer>().material = mat;
            float f = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0, 0, f);
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            IsLocalPlayer = isLocal;
            PlayerID = counter++;
            started = true;
            StartTime = Time.time;
            if (IsLocalPlayer)
            {
                MarkerManager.Instance.Play(transform);
            }
        }
    }
}
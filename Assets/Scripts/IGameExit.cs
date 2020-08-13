using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1.MirrorBlog
{
    /// <summary>
    /// ゲームから出る時の処理を実装するインターフェース
    /// </summary>
    public interface IGameExit
    {
        void Exit();
    }
}
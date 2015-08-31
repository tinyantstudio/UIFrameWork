using UnityEngine;
using System.Collections;
using System;

namespace CoolGame
{
    /// <summary>
    /// 窗口动画
    /// </summary>
    interface IWindowAnimation
    {
        /// <summary>
        /// 显示动画
        /// </summary>
        void EnterAnimation(EventDelegate.Callback onComplete);
        
        /// <summary>
        /// 隐藏动画
        /// </summary>
        void QuitAnimation(EventDelegate.Callback onComplete);
        
        /// <summary>
        /// 重置动画
        /// </summary>
        void ResetAnimation();
    }
}


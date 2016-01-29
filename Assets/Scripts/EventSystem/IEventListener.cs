using UnityEngine;
using System.Collections;

namespace TinyFrameWork
{
    /// <summary>
    /// 事件监听接口
    /// </summary>
    interface IEventListener
    {
        void RegisterEvent();
        void UnRegisterEvent();
    } 
}


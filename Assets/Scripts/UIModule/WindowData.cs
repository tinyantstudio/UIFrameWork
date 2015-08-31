using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoolGame
{
    /// <summary>
    /// 窗口Data
    /// 1.显示方式
    /// 2.窗口类型
    /// </summary>
    public class WindowData {

        // 是否是导航起始窗口(到该界面需要重置导航信息)
        public bool isStartWindow = false; 
        public UIWindowType windowType = UIWindowType.Normal;
        public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;
        public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;
    }

    public class BackWindowSequenceData
    {
        public UIBaseWindow hideTargetWindow;
        public List<WindowID> backShowTargets;
    }

    public class ShowWindowData 
    {
        // Reset窗口
        public bool forceResetWindow = false;
        // Clear导航信息
        public bool forceClearBackSeqData = false;
        // Object 数据
        public object data;
    }

    public delegate bool BoolDelegate();
}


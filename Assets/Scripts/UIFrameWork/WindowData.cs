using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    /// <summary>
    /// Define the Window core data
    /// 1. windowType
    /// 2. showMode
    /// 3. colliderMode
    /// !!You must init the window's core data in the InitWindowData !!
    /// </summary>
    public class WindowData
    {
        // If target window is mark as StartWindow force clear all the navigation sequence data
        // Your start Game MainMenu always the startWindow
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
        // Reset window
        public bool forceResetWindow = false;
        // force clear the navigation data
        public bool forceClearBackSeqData = false;
        // Object (pass data to target showed window)
        public object data;
    }

    public delegate bool BoolDelegate();
}


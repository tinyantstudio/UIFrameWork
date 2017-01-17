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
    public class WindowCoreData
    {
        // If target window is mark as [forceClearNavigation] force clear all the navigation sequence data
        // Your start Game MainMenu always the force clear navigation
        public bool forceClearNavigation = false;
        public UIWindowType windowType = UIWindowType.Normal;
        public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;
        public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;
        public UIWindowNavigationMode navigationMode = UIWindowNavigationMode.IgnoreNavigation;
    }

    public class NavigationData
    {
        public UIWindowBase hideTargetWindow;
        public List<WindowID> backShowTargets;
    }
    public delegate bool BoolDelegate();
}
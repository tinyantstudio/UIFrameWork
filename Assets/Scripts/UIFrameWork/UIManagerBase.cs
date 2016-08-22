using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    /// <summary>
    /// IUIManager
    ///     界面管理基类
    ///     管理子界面
    ///          
    ///
    /// </summary>
    public abstract class UIManagerBase : MonoBehaviour
    {
        protected Dictionary<int, UIBaseWindow> allWindows;
        protected Dictionary<int, UIBaseWindow> shownWindows;
        protected Stack<BackWindowSequenceData> backSequence;
        // current active base window
        protected UIBaseWindow curShownNormalWindow = null;
        // last active base window
        protected UIBaseWindow lastShownNormalWindow = null;

        // 是否等待关闭结束
        // 开启:等待界面关闭结束,处理后续逻辑
        // 关闭:不等待界面关闭结束，处理后续逻辑

        // Wait HideAnimation over
        // True: wait the window hide animation over
        // False: immediately call the complete animation finish the hide process
        protected bool isNeedWaitHideOver = false;

        // 管理的界面ID
        // protected int managedWindowId = 0;
        // Managed windowIds
        protected List<int> managedWindowIds = new List<int>();

        // 界面按MinDepth排序
        protected class CompareBaseWindow : IComparer<UIBaseWindow>
        {
            public int Compare(UIBaseWindow left, UIBaseWindow right)
            {
                return left.MinDepth - right.MinDepth;
            }
        }

        // Cached list avoid (always new List<WindowID>)
        private List<WindowID> listCached = new List<WindowID>();

        protected virtual void Awake()
        {
            if (allWindows == null)
                allWindows = new Dictionary<int, UIBaseWindow>();
            if (shownWindows == null)
                shownWindows = new Dictionary<int, UIBaseWindow>();
            if (backSequence == null)
                backSequence = new Stack<BackWindowSequenceData>();
        }

        public virtual UIBaseWindow GetGameWindow(WindowID id)
        {
            if (!IsWindowInControl(id))
                return null;
            if (allWindows.ContainsKey((int)id))
                return allWindows[(int)id];
            else
                return null;
        }

        public virtual T GetGameWindowScript<T>(WindowID id) where T : UIBaseWindow
        {
            UIBaseWindow baseWindow = GetGameWindow(id);
            if (baseWindow != null)
                return (T)baseWindow;
            return (T)((object)null);
        }

        /// <summary>
        /// Init the window Manager
        /// </summary>
        public virtual void InitWindowManager()
        {
            if (allWindows != null)
                allWindows.Clear();
            if (shownWindows != null)
                shownWindows.Clear();
            if (backSequence != null)
                backSequence.Clear();
        }

        /// <summary>
        /// Show target Window
        /// </summary>
        public virtual void ShowWindow(WindowID id, ShowWindowData data = null)
        {
        }

        /// <summary>
        /// Delay to show target window
        /// </summary>
        /// <param name="delayTime"> delayTime</param>
        /// <param name="id"> WindowId</param>
        /// <param name="data">show window data</param>
        public virtual void ShowWindowDelay(float delayTime, WindowID id, ShowWindowData data = null)
        {
            StartCoroutine(_ShowWindowDelay(delayTime, id, data));
        }

        private IEnumerator _ShowWindowDelay(float delayTime, WindowID id, ShowWindowData data = null)
        {
            yield return new WaitForSeconds(delayTime);
            ShowWindow(id, data);
        }

        protected virtual UIBaseWindow ReadyToShowBaseWindow(WindowID id, ShowWindowData showData = null)
        {
            return null;
        }

        protected virtual void RealShowWindow(UIBaseWindow baseWindow, WindowID id)
        {
            baseWindow.ShowWindow();
            shownWindows[(int)id] = baseWindow;
            if (baseWindow.windowData.windowType == UIWindowType.Normal)
            {
                // 改变当前显示Normal窗口
                lastShownNormalWindow = curShownNormalWindow;
                curShownNormalWindow = baseWindow;
            }
        }

        /// <summary>
        /// Navigation reShow target windows
        /// </summary>
        protected void ShowWindowForNavigation(WindowID id)
        {
            if (!this.IsWindowInControl(id))
            {
                Debuger.Log("## Current UI Manager has no control power of " + id.ToString());
                return;
            }
            if (shownWindows.ContainsKey((int)id))
                return;

            UIBaseWindow baseWindow = GetGameWindow(id);
            baseWindow.ShowWindow();
            shownWindows[(int)baseWindow.GetID] = baseWindow;
        }

        /// <summary>
        /// Hide target window
        /// </summary>
        public virtual void HideWindow(WindowID id, Action onCompleted = null)
        {
            CheckDirectlyHide(id, onCompleted);
        }

        /// <summary>
        /// Check condition to hide the target window
        /// </summary>
        protected virtual void CheckDirectlyHide(WindowID id, Action onComplete)
        {
            if (!IsWindowInControl(id))
            {
                Debug.Log("## Current UI Manager has no control power of " + id.ToString());
                return;
            }
            if (!shownWindows.ContainsKey((int)id))
                return;

            if (!isNeedWaitHideOver)
            {
                if (onComplete != null)
                    onComplete();

                shownWindows[(int)id].HideWindow(null);
                shownWindows.Remove((int)id);
                return;
            }

            if (shownWindows.ContainsKey((int)id))
            {
                if (onComplete != null)
                {
                    onComplete += delegate
                    {
                        shownWindows.Remove((int)id);
                    };
                    shownWindows[(int)id].HideWindow(onComplete);
                }
                else
                {
                    shownWindows[(int)id].HideWindow(onComplete);
                    shownWindows.Remove((int)id);
                }
            }
        }

        /// <summary>
        /// Each UI manager's return window logic
        /// </summary>
        public virtual bool ReturnWindow()
        {
            return false;
        }

        private bool ReturnWindowManager(UIBaseWindow baseWindow)
        {
            // Recursion call to return windowManager
            // if the current window has windowManager just call current's windowManager ReturnWindowManager
            UIManagerBase baseWindowManager = baseWindow.GetWindowManager;
            bool isValid = false;
            if (baseWindowManager != null)
                isValid = baseWindowManager.ReturnWindow();
            return isValid;
        }

        protected bool RealReturnWindow()
        {
            if (backSequence.Count == 0)
            {
                // 如果当前BackSequenceData 不存在返回数据
                // 检测当前Window的preWindowId是否指向上一级合法指定菜单

                // if BackSequenceData is null
                // Check window's preWindowId
                // if preWindowId defined just move to target Window(preWindowId)
                if (curShownNormalWindow == null)
                    return false;
                if (ReturnWindowManager(curShownNormalWindow))
                    return true;

                WindowID preWindowId = curShownNormalWindow.GetPreWindowID;
                if (preWindowId != WindowID.WindowID_Invaild)
                {
                    HideWindow(curShownNormalWindow.GetID, delegate
                    {
                        ShowWindow(preWindowId, null);
                    });
                }
                else
                    Debug.LogWarning("## CurrentShownWindow " + curShownNormalWindow.GetID + " preWindowId is " + WindowID.WindowID_Invaild);
                return false;
            }
            BackWindowSequenceData backData = backSequence.Peek();
            if (backData != null)
            {
                if (ReturnWindowManager(backData.hideTargetWindow))
                    return true;

                WindowID hideId = backData.hideTargetWindow.GetID;
                if (backData.hideTargetWindow != null && shownWindows.ContainsKey((int)hideId))
                    HideWindow(hideId, delegate
                    {
                        if (backData.backShowTargets != null)
                        {
                            for (int i = 0; i < backData.backShowTargets.Count; i++)
                            {
                                WindowID backId = backData.backShowTargets[i];
                                ShowWindowForNavigation(backId);
                                if (i == backData.backShowTargets.Count - 1)
                                {
                                    Debuger.Log("##[UIFrameWork] Change currentShownNormalWindow : " + backId);
                                    {
                                        this.lastShownNormalWindow = this.curShownNormalWindow;
                                        this.curShownNormalWindow = GetGameWindow(backId);
                                    }
                                }
                            }
                        }
                        backSequence.Pop();
                    });
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Clear the back sequence data
        /// </summary>
        public void ClearBackSequence()
        {
            if (backSequence != null)
                backSequence.Clear();
        }

        /// <summary>
        /// Destroy all window
        /// </summary>
        public virtual void ClearAllWindow()
        {
            if (allWindows != null)
            {
                foreach (KeyValuePair<int, UIBaseWindow> window in allWindows)
                {
                    UIBaseWindow baseWindow = window.Value;
                    baseWindow.DestroyWindow();
                }
                allWindows.Clear();
                shownWindows.Clear();
                backSequence.Clear();
            }
        }

        protected void HideAllShownWindow(bool includeFixed = true)
        {
            listCached.Clear();
            if (!includeFixed)
            {
                foreach (KeyValuePair<int, UIBaseWindow> window in shownWindows)
                {
                    if (window.Value.windowData.windowType == UIWindowType.Fixed)
                        continue;
                    listCached.Add((WindowID)window.Key);
                    window.Value.HideWindowDirectly();
                }

                if (listCached.Count > 0)
                {
                    for (int i = 0; i < listCached.Count; i++)
                        shownWindows.Remove((int)listCached[i]);
                }
            }
            else
            {
                foreach (KeyValuePair<int, UIBaseWindow> window in shownWindows)
                    window.Value.HideWindowDirectly();
                shownWindows.Clear();
            }
        }

        // check window control
        protected bool IsWindowInControl(WindowID id)
        {
            //int targetId = 1 << ((int)id);
            //return ((managedWindowId & targetId) == targetId);
            return this.managedWindowIds.Contains((int)id);
        }

        // add window to target manager
        protected void AddWindowInControl(WindowID id)
        {
            //int targetId = 1 << ((int)id);
            //managedWindowId |= targetId;
            this.managedWindowIds.Add((int)id);
        }

        // init the Manager's control window
        protected abstract void InitWindowControl();
        public virtual void ResetAllInControlWindows()
        {
        }
    }
}
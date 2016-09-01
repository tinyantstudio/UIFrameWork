using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    /// <summary>
    /// IUIManager
    ///     Base UIManager
    ///     Manager some sub window(UIRankManager for UIRankDetailWindow and UIRankOwnDetailWindow)
    ///     NOTE:UICenterMasterManager is the Center key master for manager all parent window(UIRank UISkill and so on)
    ///
    /// </summary>
    public abstract class UIManagerBase : MonoBehaviour
    {
        protected Dictionary<int, UIBaseWindow> allWindows;
        protected Dictionary<int, UIBaseWindow> shownWindows;
        protected Stack<BackWindowSequenceData> backSequence;
        // current active navigation window
        protected UIBaseWindow curNavigationWindow = null;
        // last active navigation window
        protected UIBaseWindow lastNavigationWindow = null;
        // current shown window
        protected WindowID curShownWindowId = WindowID.WindowID_Invaild;
        // Wait HideAnimation over
        // True: wait the window hide animation over
        // False: immediately call the complete animation finish the hide process
        protected bool isNeedWaitHideOver = false;

        // Managed windowIds
        protected List<int> managedWindowIds = new List<int>();

        // Compare with panel depth
        protected class CompareBaseWindow : IComparer<UIBaseWindow>
        {
            public int Compare(UIBaseWindow left, UIBaseWindow right)
            {
                return left.MinDepth - right.MinDepth;
            }
        }

        // Cached list (avoid always new List<WindowID>)
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
        /// <param name="showData">show window data</param>
        public virtual void ShowWindowDelay(float delayTime, WindowID id, ShowWindowData showData = null)
        {
            StopAllCoroutines();
            StartCoroutine(_ShowWindowDelay(delayTime, id, showData));
        }

        private IEnumerator _ShowWindowDelay(float delayTime, WindowID id, ShowWindowData showData = null)
        {
            yield return new WaitForSeconds(delayTime);
            ShowWindow(id, showData);
        }

        protected virtual UIBaseWindow ReadyToShowBaseWindow(WindowID id, ShowWindowData showData = null)
        {
            return null;
        }

        protected virtual void RealShowWindow(UIBaseWindow baseWindow, WindowID id, ShowWindowData showData = null)
        {
            BaseWindowContextData contextData = showData == null ? null : showData.contextData;
            baseWindow.ShowWindow(contextData);
            shownWindows[(int)id] = baseWindow;
            if (baseWindow.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation)
            {
                lastNavigationWindow = curNavigationWindow;
                curNavigationWindow = baseWindow;
            }
            Debug.Log("#### Show the window : " + baseWindow.ID.ToString());
            this.curShownWindowId = baseWindow.ID;
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
            shownWindows[(int)baseWindow.ID] = baseWindow;
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
                Debuger.Log("## Current UI Manager has no control power of " + id.ToString());
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
                if (curNavigationWindow == null)
                    return false;
                if (ReturnWindowManager(curNavigationWindow))
                    return true;

                // if curNavigationWindow BackSequenceData is null
                // Check window's preWindowId
                // if preWindowId defined just move to target Window(preWindowId)
                WindowID preWindowId = curNavigationWindow.PreWindowID;
                if (preWindowId != WindowID.WindowID_Invaild)
                {
                    Debuger.LogWarning(string.Format(string.Format("## Current nav window {0} need show pre window {1}.", curNavigationWindow.ID.ToString(), preWindowId.ToString())));
                    HideWindow(curNavigationWindow.ID, delegate
                    {
                        ShowWindowData showData = new ShowWindowData();
                        showData.executeNavLogic = false;
                        ShowWindow(preWindowId, showData);
                    });
                }
                else
                {
                    Debuger.LogWarning("## CurrentShownWindow " + curNavigationWindow.ID + " preWindowId is " + WindowID.WindowID_Invaild);
                }
                return false;
            }
            BackWindowSequenceData backData = backSequence.Peek();
            if (backData != null)
            {
                if (ReturnWindowManager(backData.hideTargetWindow))
                    return true;

                WindowID hideId = backData.hideTargetWindow.ID;
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
                                    this.lastNavigationWindow = this.curNavigationWindow;
                                    this.curNavigationWindow = GetGameWindow(backId);
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
            return this.managedWindowIds.Contains((int)id);
        }

        // add window to target manager
        protected void AddWindowInControl(WindowID id)
        {
            this.managedWindowIds.Add((int)id);
        }

        // init the Manager's control window
        protected abstract void InitWindowControl();
        public virtual void ResetAllInControlWindows()
        {
        }
    }
}
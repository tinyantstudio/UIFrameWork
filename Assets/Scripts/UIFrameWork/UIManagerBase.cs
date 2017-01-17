using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        protected Dictionary<int, UIWindowBase> dicAllWindows;
        protected Dictionary<int, UIWindowBase> dicShownWindows;
        protected Stack<NavigationData> backSequence;
        // current active navigation window
        protected UIWindowBase curNavigationWindow = null;
        // last active navigation window
        protected UIWindowBase lastNavigationWindow = null;
        // Wait HideAnimation over
        // True: wait the window hide animation over
        // False: immediately call the complete animation finish the hide process
        protected bool isNeedWaitHideOver = false;

        // Managed windowIds
        protected List<int> managedWindowIds = new List<int>();

        // Compare with panel depth
        protected class CompareBaseWindow : IComparer<UIWindowBase>
        {
            public int Compare ( UIWindowBase left, UIWindowBase right )
            {
                return left.MinDepth - right.MinDepth;
            }
        }

        // Cached list (avoid always new List<WindowID>)
        private List<WindowID> listCached = new List<WindowID>();

        protected virtual void Awake ()
        {
            if (dicAllWindows == null)
                dicAllWindows = new Dictionary<int, UIWindowBase>();
            if (dicShownWindows == null)
                dicShownWindows = new Dictionary<int, UIWindowBase>();
            if (backSequence == null)
                backSequence = new Stack<NavigationData>();
        }

        public virtual UIWindowBase GetGameWindow ( WindowID id )
        {
            if (!IsWindowInControl(id))
                return null;
            if (dicAllWindows.ContainsKey((int) id))
                return dicAllWindows[(int) id];
            else
                return null;
        }

        public virtual T GetGameWindowScript<T> ( WindowID id ) where T : UIWindowBase
        {
            UIWindowBase baseWindow = GetGameWindow(id);
            if (baseWindow != null)
                return (T) baseWindow;
            return (T) ((object) null);
        }

        /// <summary>
        /// Init the window Manager
        /// </summary>
        public virtual void InitWindowManager ()
        {
            if (dicAllWindows != null)
                dicAllWindows.Clear();
            if (dicShownWindows != null)
                dicShownWindows.Clear();
            if (backSequence != null)
                backSequence.Clear();
        }

        /// <summary>
        /// Show target Window
        /// </summary>
        public virtual void ShowWindow ( WindowID id, ShowWindowData data = null )
        {
        }

        /// <summary>
        /// Delay to show target window
        /// </summary>
        /// <param name="delayTime"> delayTime</param>
        /// <param name="id"> WindowId</param>
        /// <param name="showData">show window data</param>
        public virtual void ShowWindowDelay ( float delayTime, WindowID id, ShowWindowData showData = null )
        {
            StopAllCoroutines();
            StartCoroutine(_ShowWindowDelay(delayTime, id, showData));
        }

        private IEnumerator _ShowWindowDelay ( float delayTime, WindowID id, ShowWindowData showData = null )
        {
            yield return new WaitForSeconds(delayTime);
            ShowWindow(id, showData);
        }

        protected virtual UIWindowBase ReadyToShowBaseWindow ( WindowID id, ShowWindowData showData = null )
        {
            return null;
        }

        protected virtual void RealShowWindow ( UIWindowBase baseWindow, WindowID id, ShowWindowData showData = null )
        {
            BaseWindowContextData contextData = showData == null ? null : showData.contextData;
            baseWindow.ShowWindow(contextData);
            dicShownWindows[(int) id] = baseWindow;
            if (baseWindow.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation)
            {
                lastNavigationWindow = curNavigationWindow;
                curNavigationWindow = baseWindow;
                Debuger.Log("<color=magenta>### current Navigation window </color>" + baseWindow.ID.ToString());
            }
        }

        /// <summary>
        /// Hide target window
        /// </summary>
        public virtual void HideWindow ( WindowID id, Action onCompleted = null )
        {
            CheckDirectlyHide(id, onCompleted);
        }

        /// <summary>
        /// Check condition to hide the target window
        /// </summary>
        protected void CheckDirectlyHide ( WindowID id, Action onComplete )
        {
            if (!IsWindowInControl(id))
            {
                Debuger.Log("## Current UI Manager has no control power of " + id.ToString());
                return;
            }
            if (!dicShownWindows.ContainsKey((int) id))
                return;

            if (!isNeedWaitHideOver)
            {
                if (onComplete != null)
                    onComplete();

                dicShownWindows[(int) id].HideWindow(null);
                dicShownWindows.Remove((int) id);
                return;
            }

            if (dicShownWindows.ContainsKey((int) id))
            {
                if (onComplete != null)
                {
                    onComplete += delegate
                    {
                        dicShownWindows.Remove((int) id);
                    };
                    dicShownWindows[(int) id].HideWindow(onComplete);
                }
                else
                {
                    dicShownWindows[(int) id].HideWindow(onComplete);
                    dicShownWindows.Remove((int) id);
                }
            }
        }

        /// <summary>
        /// Each UI manager's return window logic
        /// </summary>
        public virtual bool PopNavigationWindow ()
        {
            return false;
        }

        private bool PopUpWindowManager ( UIWindowBase baseWindow )
        {
            // Recursion call to return windowManager
            // if the current window has windowManager just call current's windowManager PopUpWindowManager
            UIManagerBase baseWindowManager = baseWindow.GetWindowManager;
            bool isValid = false;
            if (baseWindowManager != null)
                isValid = baseWindowManager.PopNavigationWindow();
            return isValid;
        }

        protected bool RealPopNavigationWindow ()
        {
            if (backSequence.Count == 0)
            {
                if (curNavigationWindow == null)
                    return false;
                if (PopUpWindowManager(curNavigationWindow))
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
            NavigationData backData = backSequence.Peek();
            if (backData != null)
            {
                // check the current navigation data
                int curId = this.GetCurrentShownWindow();
                if (curId != (int) backData.hideTargetWindow.ID)
                {
                    Debuger.Log("<color=red>Can't PopUp seq data [backData.hideTargetWindow.ID != this.curShownWindowId]</color>");
                    return false;
                }

                if (PopUpWindowManager(backData.hideTargetWindow))
                    return true;

                WindowID hideId = backData.hideTargetWindow.ID;
                if (!dicShownWindows.ContainsKey((int) hideId))
                    ExectuteBackSeqData(backData);
                else
                    HideWindow(hideId, delegate
                    {
                        ExectuteBackSeqData(backData);
                    });
            }
            return true;
        }

        private void ExectuteBackSeqData ( NavigationData nd )
        {
            // 
            // stack count check
            ///
            if (backSequence.Count > 0)
                backSequence.Pop();
            if (nd.backShowTargets == null)
                return;

            for (int i = 0; i < nd.backShowTargets.Count; i++)
            {
                WindowID backId = nd.backShowTargets[i];
                ShowWindowForNavigation(backId);
                if (i == nd.backShowTargets.Count - 1)
                {
                    UIWindowBase window = GetGameWindow(backId);
                    if (window.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation)
                    {
                        this.lastNavigationWindow = this.curNavigationWindow;
                        this.curNavigationWindow = window;
                        Debuger.Log("<color=magenta>##[UIManagerBase return window]##</color> Change currentShownNormalWindow : " + backId);
                    }
                }
            }
        }

        /// <summary>
        /// Navigation reShow target windows
        /// </summary>
        private void ShowWindowForNavigation ( WindowID id )
        {
            if (!this.IsWindowInControl(id))
            {
                Debuger.Log("## Current UI Manager has no control power of " + id.ToString());
                return;
            }
            if (dicShownWindows.ContainsKey((int) id))
                return;

            UIWindowBase baseWindow = GetGameWindow(id);
            baseWindow.ShowWindow();
            dicShownWindows[(int) baseWindow.ID] = baseWindow;
        }

        /// <summary>
        /// Clear the back sequence data
        /// </summary>
        public void ClearBackSequence ()
        {
            if (backSequence != null)
                backSequence.Clear();
        }

        protected CompareBaseWindow compareWindowFun = new CompareBaseWindow();
        protected virtual int GetCurrentShownWindow ()
        {
            // default window min depth
            List<UIWindowBase> listWnds = this.dicShownWindows.Values.ToList();
            listWnds.Sort(this.compareWindowFun);
            for (int i = listWnds.Count - 1; i >= 0; i--)
            {
                if (listWnds[i].windowData.windowType != UIWindowType.Fixed)
                    return (int) (listWnds[i].ID);
            }
            return (int) WindowID.WindowID_Invaild;
        }

        /// <summary>
        /// Destroy all window
        /// </summary>
        public virtual void ClearAllWindow ()
        {
            if (dicAllWindows != null)
            {
                foreach (KeyValuePair<int, UIWindowBase> window in dicAllWindows)
                {
                    UIWindowBase baseWindow = window.Value;
                    baseWindow.DestroyWindow();
                }
                dicAllWindows.Clear();
                dicShownWindows.Clear();
                backSequence.Clear();
            }
        }

        public void HideAllShownWindow ( bool includeFixed = false )
        {
            listCached.Clear();
            foreach (KeyValuePair<int, UIWindowBase> window in dicShownWindows)
            {
                if (window.Value.windowData.windowType == UIWindowType.Fixed && !includeFixed)
                    continue;
                listCached.Add((WindowID) window.Key);
                window.Value.HideWindowDirectly();
            }

            if (listCached.Count > 0)
            {
                for (int i = 0; i < listCached.Count; i++)
                    dicShownWindows.Remove((int) listCached[i]);
            }
        }

        // check window control
        protected bool IsWindowInControl ( WindowID id )
        {
            return this.managedWindowIds.Contains((int) id);
        }

        // add window to target manager
        protected void AddWindowInControl ( WindowID id )
        {
            this.managedWindowIds.Add((int) id);
        }

        // init the Manager's control window
        protected abstract void InitWindowControl ();
        public virtual void ResetAllInControlWindows ()
        {
        }


        #region Methods for Other Window except !!UICenterMasterManager!!
        // Deal with Navigation sequence data Look At UIRankManager example
        // you can change Navigation data when pop up window 
        // !! Not used for UICenterMasterManager !!
        // Just for Sub window Manager
        // 
        // Fixed Bug : When you close the navigation window by CloseBtn may cause the Navigation Data add more time
        // Check Navigation data when Call Manager's PopNavigationWindow, we just add DealWithNavigationWhenPopWindow method for deal with navigation when pop up window
        // 
        protected virtual void DealWithNavigationWhenPopWindow ()
        {
            if (dicShownWindows.Count <= 0)
                return;

            for (int i = 0; i < this.managedWindowIds.Count; i++)
            {
                int wndId = managedWindowIds[i];
                if (!dicShownWindows.ContainsKey(wndId))
                    continue;
                UIWindowBase wnd = dicShownWindows[wndId];
                if (wnd.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation &&
                    !wnd.IsLock)
                {
                    NavigationData nd = new NavigationData();
                    nd.hideTargetWindow = wnd;
                    backSequence.Push(nd);
                    Debug.Log("## Push new navigation data " + ((WindowID) wndId).ToString());
                    break;
                }
            }
        }

        protected void ShowWindowForOtherWindowManager ( WindowID id, ShowWindowData showData )
        {
            if (!IsWindowInControl(id))
            {
                Debuger.Log("UIRankManager has no control power of " + id.ToString());
                return;
            }
            if (dicShownWindows.ContainsKey((int) id))
                return;
            if (dicAllWindows.ContainsKey((int) id))
            {
                UIWindowBase baseWindow = dicAllWindows[(int) id];
                if (baseWindow.ID != id)
                {
                    Debuger.LogError(string.Format("[UIRankManager BaseWindowId :{0} != shownWindowId :{1}]", baseWindow.ID, id));
                    return;
                }
                this.RealShowWindow(baseWindow, baseWindow.ID, showData);
            }
        }
        #endregion
    }
}
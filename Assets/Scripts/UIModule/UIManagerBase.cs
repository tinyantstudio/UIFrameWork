using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoolGame
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
        protected Dictionary<WindowID, UIBaseWindow> allWindows;
        protected Dictionary<WindowID, UIBaseWindow> shownWindows;
        protected Stack<BackWindowSequenceData> backSequence;
        // 当前显示活跃界面
        protected UIBaseWindow curShownNormalWindow = null;
        // 上一活跃界面
        protected UIBaseWindow lastShownNormalWindow = null;

        // 是否等待关闭结束
        // 开启:等待界面关闭结束,处理后续逻辑
        // 关闭:不等待界面关闭结束，处理后续逻辑
        protected bool isNeedWaitHideOver = false;

        // 管理的界面ID
        protected int managedWindowId = 0;

        // 界面按MinDepth排序
        protected class CompareBaseWindow : IComparer<UIBaseWindow>
        {
            public int Compare(UIBaseWindow left, UIBaseWindow right)
            {
                return left.MinDepth - right.MinDepth;
            }
        }

        protected virtual void Awake()
        {
            if (allWindows == null)
                allWindows = new Dictionary<WindowID, UIBaseWindow>();
            if (shownWindows == null)
                shownWindows = new Dictionary<WindowID, UIBaseWindow>();
            if (backSequence == null)
                backSequence = new Stack<BackWindowSequenceData>();
        }

        public virtual UIBaseWindow GetGameWindow(WindowID id)
        {
            if (!IsWindowInControl(id))
                return null;
            if (allWindows.ContainsKey(id))
                return allWindows[id];
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
        /// 初始化当前界面管理类
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
        /// 显示界面
        /// </summary>
        /// <param name="id">界面ID</param>
        public virtual void ShowWindow(WindowID id, ShowWindowData data = null)
        {

        }

        /// <summary>
        /// Delay 显示界面
        /// </summary>
        /// <param name="delayTime"> 延迟时间</param>
        /// <param name="id"> 界面ID</param>
        /// <param name="data">显示数据</param>
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

        /// <summary>
        /// 显示界面，方面在现实之前做其他操作
        /// </summary>
        protected virtual void RealShowWindow(UIBaseWindow baseWindow, WindowID id)
        {
            baseWindow.ShowWindow();
            shownWindows[id] = baseWindow;
            if (baseWindow.windowData.windowType == UIWindowType.Normal)
            {
                // 改变当前显示Normal窗口
                lastShownNormalWindow = curShownNormalWindow;
                curShownNormalWindow = baseWindow; 
            }
        }

        // 直接打开窗口
        protected void ShowWindowForBack(WindowID id)
        {
            // 检测控制权限
            if (!this.IsWindowInControl(id))
            {
                Debug.Log("UIManager has no control power of " + id.ToString());
                return;
            }
            if (shownWindows.ContainsKey(id))
                return;

            UIBaseWindow baseWindow = GetGameWindow(id);
            baseWindow.ShowWindow();
            shownWindows[baseWindow.GetID] = baseWindow;

        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        /// <param name="id"></param>
        public virtual void HideWindow(WindowID id, Action onCompleted = null)
        {
            CheckDirectlyHide(id, onCompleted);
        }

        protected virtual void CheckDirectlyHide(WindowID id, Action onComplete)
        {
            if (!IsWindowInControl(id))
            {
                Debug.Log("UIRankManager has no control power of " + id.ToString());
                return;
            }
            if (!shownWindows.ContainsKey(id))
                return;

            if (!isNeedWaitHideOver)
            {
                if (onComplete != null)
                    onComplete();

                shownWindows[id].HideWindow(null);
                shownWindows.Remove(id);
                return;
            }

            if (shownWindows.ContainsKey(id))
            {
                if (onComplete != null)
                {
                    onComplete += delegate
                    {
                        shownWindows.Remove(id);
                    };
                    shownWindows[id].HideWindow(onComplete);
                }
                else
                {
                    shownWindows[id].HideWindow(onComplete);
                    shownWindows.Remove(id);
                }
            }
        }

        /// <summary>
        /// 返回逻辑
        /// </summary>
        public virtual bool ReturnWindow()
        {
            return false;
        }

        private bool ReturnWindowManager(UIBaseWindow baseWindow)
        {
            // 退出当前界面子界面
            UIManagerBase baseWindowManager = baseWindow.GetWindowManager;
            bool isValid = false;
            if (baseWindowManager != null)
                isValid = baseWindowManager.ReturnWindow();
            return isValid;
        }

        // 界面导航返回
        protected bool RealReturnWindow()
        {
            if (backSequence.Count == 0)
            {
                // 如果当前BackSequenceData 不存在返回数据
                // 检测当前Window的preWindowId是否指向上一级合法菜单
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
                    Debug.LogWarning("currentShownWindow " + curShownNormalWindow.GetID + " preWindowId is " + WindowID.WindowID_Invaild);
                return false;
            }
            BackWindowSequenceData backData = backSequence.Peek();
            if (backData != null)
            {
                // 退出当前界面子界面
                if (ReturnWindowManager(backData.hideTargetWindow))
                    return true;

                WindowID hideId = backData.hideTargetWindow.GetID;
                if (backData.hideTargetWindow != null && shownWindows.ContainsKey(hideId))
                    HideWindow(hideId, delegate
                    {
                        if (backData.backShowTargets != null)
                        {
                            for (int i = 0; i < backData.backShowTargets.Count; i++)
                            {
                                WindowID backId = backData.backShowTargets[i];
                                ShowWindowForBack(backId);
                                if (i == backData.backShowTargets.Count - 1)
                                {
                                    Debug.Log("change currentShownNormalWindow : " + backId);
                                    {
                                        // 改变当前活跃Normal窗口
                                        this.lastShownNormalWindow = this.curShownNormalWindow;
                                        this.curShownNormalWindow = GetGameWindow(backId); 
                                    }
                                }
                            }
                        }
                        
                        // 隐藏当前界面
                        backSequence.Pop();
                    });
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 清空导航信息
        /// </summary>
        public void ClearBackSequence()
        {
            if (backSequence != null)
                backSequence.Clear();
        }

        /// <summary>
        /// 清空所有界面
        /// </summary>
        public virtual void ClearAllWindow()
        {
            if (allWindows != null)
            {
                foreach (KeyValuePair<WindowID, UIBaseWindow> window in allWindows)
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
            List<WindowID> removedKey = null;

            if (!includeFixed)
            {
                foreach (KeyValuePair<WindowID, UIBaseWindow> window in shownWindows)
                {
                    if (window.Value.windowData.windowType == UIWindowType.Fixed)
                        continue;

                    if (removedKey == null)
                        removedKey = new List<WindowID>();

                    removedKey.Add(window.Key);
                    window.Value.HideWindowDirectly();
                }

                if (removedKey != null)
                {
                    for (int i = 0; i < removedKey.Count; i++)
                        shownWindows.Remove(removedKey[i]);
                }
            }
            else
            {
                foreach (KeyValuePair<WindowID, UIBaseWindow> window in shownWindows)
                    window.Value.HideWindowDirectly();
                shownWindows.Clear();
            }
        }

        protected bool IsWindowInControl(WindowID id)
        {
            int targetId = 1 << ((int)id);
            return ((managedWindowId & targetId) == targetId);
        }

        protected void AddWindowInControl(WindowID id)
        {
            int targetId = 1 << ((int)id);
            managedWindowId |= targetId;
        }

        protected abstract void InitWindowControl();
        public virtual void ResetAllInControlWindows()
        {
        }
    }
}
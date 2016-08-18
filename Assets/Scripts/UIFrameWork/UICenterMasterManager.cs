using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    /// <summary>
    /// UI Main Manager "the Master" most important Class
    ///         Control all the "Big Parent" window:UIRank,UIShop,UIGame,UIMainMenu and so on.
    ///         UIRankManager: control the rank window logic (UIRankDetail sub window)
    ///         May be UIShopManager:control the UIShopDetailWindow or UIShopSubTwoWindow
    /// 
    /// 枢纽中心，控制整个大界面的显示逻辑UIRank，UIMainMenu等
    ///         UIRank排行榜界面可能也会有自己的Manager用来管理Rank系统中自己的子界面，这些子界面不交给"老大"UICenterMasterManager管理
    ///         分而治之，不是中央集权
    /// </summary>
    public class UICenterMasterManager : UIManagerBase
    {
        // save the UIRoot
        public Transform UIRoot;
        // NormalWindow node
        [System.NonSerialized]
        public Transform UINormalWindowRoot;
        // PopUpWindow node
        [System.NonSerialized]
        public Transform UIPopUpWindowRoot;
        // FixedWindow node
        [System.NonSerialized]
        public Transform UIFixedWidowRoot;

        // Each Type window start Depth
        private int fixedWindowDepth = 100;     
        private int popUpWindowDepth = 150;     
        private int normalWindowDepth = 2;       

        // Atlas reference
        // Mask Atlas for sprite mask(Common Collider window background)
        public UIAtlas maskAtlas;

        private static UICenterMasterManager instance;
        public static UICenterMasterManager GetInstance()
        {
            return instance;
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            InitWindowManager();
            Debug.Log("UIManager is call awake.");
        }

        public override void ShowWindow(WindowID id, ShowWindowData data = null)
        {
            UIBaseWindow baseWindow = ReadyToShowBaseWindow(id, data);
            if (baseWindow != null)
            {
                RealShowWindow(baseWindow, id);

                // 是否清空当前导航信息(回到主菜单)
                // If target window is mark as StartWindow force clear all the navigation sequence data
                if (baseWindow.windowData.isStartWindow)
                {
                    Debuger.Log("[Enter the start window, reset the backSequenceData for the navigation system.]");
                    ClearBackSequence();
                }

                if (data != null && data.forceClearBackSeqData)
                    ClearBackSequence();
            }
        }


        protected override UIBaseWindow ReadyToShowBaseWindow(WindowID id, ShowWindowData showData = null)
        {
            // 检测控制权限
            if (!this.IsWindowInControl(id))
            {
                Debug.Log("UIManager has no control power of " + id.ToString());
                return null;
            }
            if (shownWindows.ContainsKey((int)id))
                return null;

            UIBaseWindow baseWindow = GetGameWindow(id);
            bool newAdded = false;
            if (!baseWindow)
            {
                newAdded = true;
                if (UIResourceDefine.windowPrefabPath.ContainsKey((int)id))
                {
                    string prefabPath = UIResourceDefine.UIPrefabPath + UIResourceDefine.windowPrefabPath[(int)id];
                    GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    if (prefab != null)
                    {
                        GameObject uiObject = (GameObject)GameObject.Instantiate(prefab);
                        NGUITools.SetActive(uiObject, true);
                        baseWindow = uiObject.GetComponent<UIBaseWindow>();
                        // 需要动态添加对应的控制界面,prefab不用添加脚本
                        Transform targetRoot = GetTargetRoot(baseWindow.windowData.windowType);
                        GameUtility.AddChildToTarget(targetRoot, baseWindow.gameObject.transform);
                        allWindows[(int)id] = baseWindow;
                    }
                }
            }

            if (baseWindow == null)
                Debug.LogError("[window instance is null.]" + id.ToString());

            // 重置界面(第一次添加，强制Reset)
            if (newAdded || (showData != null && showData.forceResetWindow))
                baseWindow.ResetWindow();

            // 显示界面固定内容


            // 导航系统数据更新
            RefreshBackSequenceData(baseWindow);
            // 调整层级depth
            AdjustBaseWindowDepth(baseWindow);
            // 添加背景Collider
            AddColliderBgForWindow(baseWindow);
            return baseWindow;
        }

        public override void HideWindow(WindowID id, Action onComplete = null)
        {
            CheckDirectlyHide(id, onComplete);
        }

        public override void InitWindowManager()
        {
            base.InitWindowManager();
            InitWindowControl();
            isNeedWaitHideOver = true;

            DontDestroyOnLoad(UIRoot);

            if (UIFixedWidowRoot == null)
            {
                UIFixedWidowRoot = new GameObject("UIFixedWidowRoot").transform;
                GameUtility.AddChildToTarget(UIRoot, UIFixedWidowRoot);
                GameUtility.ChangeChildLayer(UIFixedWidowRoot, UIRoot.gameObject.layer);
            }
            if (UIPopUpWindowRoot == null)
            {
                UIPopUpWindowRoot = new GameObject("UIPopUpWindowRoot").transform;
                GameUtility.AddChildToTarget(UIRoot, UIPopUpWindowRoot);
                GameUtility.ChangeChildLayer(UIPopUpWindowRoot, UIRoot.gameObject.layer);
            }
            if (UINormalWindowRoot == null)
            {
                UINormalWindowRoot = new GameObject("UINormalWindowRoot").transform;
                GameUtility.AddChildToTarget(UIRoot, UINormalWindowRoot);
                GameUtility.ChangeChildLayer(UINormalWindowRoot, UIRoot.gameObject.layer);
            }

            // test for show two main window to start the demo.
            ShowWindow(WindowID.WindowID_MainMenu);
            ShowWindow(WindowID.WindowID_TopBar);
        }

        protected override void InitWindowControl()
        {
            this.managedWindowId = 0;
            AddWindowInControl(WindowID.WindowID_Level);
            AddWindowInControl(WindowID.WindowID_Rank);
            AddWindowInControl(WindowID.WindowID_MainMenu);
            AddWindowInControl(WindowID.WindowID_TopBar);
            AddWindowInControl(WindowID.WindowID_MessageBox);
            AddWindowInControl(WindowID.WindowID_LevelDetail);
            AddWindowInControl(WindowID.WindowID_Matching);
            AddWindowInControl(WindowID.WindowID_MatchResult);
            AddWindowInControl(WindowID.WindowID_Skill);
        }

        public override void ClearAllWindow()
        {
            base.ClearAllWindow();
        }

        /// <summary>
        /// Return logic 
        /// </summary>
        public override bool ReturnWindow()
        {
            if (curShownNormalWindow != null)
            {
                bool needReturn = curShownNormalWindow.ExecuteReturnLogic();
                if (needReturn)
                    return false;
            }
            return RealReturnWindow();
        }

        /// <summary>
        /// Calculate right depth with windowType
        /// Find next right depth
        /// </summary>
        /// <param name="baseWindow"></param>
        private void AdjustBaseWindowDepth(UIBaseWindow baseWindow)
        {
            UIWindowType windowType = baseWindow.windowData.windowType;
            int needDepth = 1;
            if (windowType == UIWindowType.Normal)
            {
                needDepth = Mathf.Clamp(GameUtility.GetMaxTargetDepth(UINormalWindowRoot.gameObject, false) + 1, normalWindowDepth, int.MaxValue);
                Debug.Log("[UIWindowType.Normal] maxDepth is " + needDepth + baseWindow.GetID);
            }
            else if (windowType == UIWindowType.PopUp)
            {
                needDepth = Mathf.Clamp(GameUtility.GetMaxTargetDepth(UIPopUpWindowRoot.gameObject) + 1, popUpWindowDepth, int.MaxValue);
                Debug.Log("[UIWindowType.PopUp] maxDepth is " + needDepth);
            }
            else if (windowType == UIWindowType.Fixed)
            {
                needDepth = Mathf.Clamp(GameUtility.GetMaxTargetDepth(UIFixedWidowRoot.gameObject) + 1, fixedWindowDepth, int.MaxValue);
                Debug.Log("[UIWindowType.Fixed] max depth is " + needDepth);
            }
            if(baseWindow.MinDepth != needDepth)
                GameUtility.SetTargetMinPanel(baseWindow.gameObject, needDepth);
            baseWindow.MinDepth = needDepth;
        }

        /// <summary>
        /// Add Collider and BgTexture for target window
        /// </summary>
        private void AddColliderBgForWindow(UIBaseWindow baseWindow)
        {
            UIWindowColliderMode colliderMode = baseWindow.windowData.colliderMode;
            if (colliderMode == UIWindowColliderMode.None)
                return;

            if (colliderMode == UIWindowColliderMode.Normal)
                GameUtility.AddColliderBgToTarget(baseWindow.gameObject, "Mask02", maskAtlas, true);
            if (colliderMode == UIWindowColliderMode.WithBg)
                GameUtility.AddColliderBgToTarget(baseWindow.gameObject, "Mask02", maskAtlas, false);
        }

        public void RefreshBackSequenceData(UIBaseWindow baseWindow)
        {
            WindowData windowData = baseWindow.windowData;
            if (baseWindow.RefreshBackSeqData)
            {
                bool dealBackSequence = true;
                if (curShownNormalWindow != null)
                {
                    if (curShownNormalWindow.windowData.showMode == UIWindowShowMode.NoNeedBack)
                    {
                        dealBackSequence = false;
                        HideWindow(curShownNormalWindow.GetID, null);
                    }
                    Debug.Log("* current shown Normal Window is " + curShownNormalWindow.GetID);
                }

                if (shownWindows.Count > 0 && dealBackSequence)
                {
                    List<WindowID> removedKey = null;
                    List<WindowID> newPushList = new List<WindowID>();
                    List<UIBaseWindow> sortByMinDepth = new List<UIBaseWindow>();

                    BackWindowSequenceData backData = new BackWindowSequenceData();

                    foreach (KeyValuePair<int, UIBaseWindow> window in shownWindows)
                    {
                        bool needToHide = true;
                        if (windowData.showMode == UIWindowShowMode.NeedBack
                            || window.Value.windowData.windowType == UIWindowType.Fixed)
                            needToHide = false;

                        if (needToHide)
                        {
                            if (removedKey == null)
                                removedKey = new List<WindowID>();
                            removedKey.Add((WindowID)window.Key);

                            // HideOther Type(hide all window directly)
                            window.Value.HideWindowDirectly();
                        }

                        // 将Window添加到BackSequence中
                        if (window.Value.CanAddedToBackSeq)
                            sortByMinDepth.Add(window.Value);
                    }

                    if (removedKey != null)
                    {
                        for (int i = 0; i < removedKey.Count; i++)
                            shownWindows.Remove((int)removedKey[i]);
                    }

                    // push to backToShowWindows stack
                    if (sortByMinDepth.Count > 0)
                    {
                        // 按照层级顺序存入显示List中 
                        // Add to return show target list
                        sortByMinDepth.Sort(new CompareBaseWindow());
                        for (int i = 0; i < sortByMinDepth.Count;i++ )
                        {
                            WindowID pushWindowId = sortByMinDepth[i].GetID;
                            newPushList.Add(pushWindowId);
                        }

                        backData.hideTargetWindow = baseWindow;
                        backData.backShowTargets = newPushList;
                        backSequence.Push(backData);
                    }
                }
            }
            else if (windowData.showMode == UIWindowShowMode.NoNeedBack)
                HideAllShownWindow(true);

            CheckBackSequenceData(baseWindow);
        }

        private void CheckBackSequenceData(UIBaseWindow baseWindow)
        {
            // 如果当前存在BackSequence数据
            // 1.栈顶界面不是当前要显示的界面需要清空BackSequence(导航被重置)
            // 2.栈顶界面是当前显示界面,如果类型为(NeedBack)则需要显示所有backShowTargets界面
            WindowData windowData = baseWindow.windowData;
            if (baseWindow.RefreshBackSeqData)
            {
                if (backSequence.Count > 0)
                {
                    BackWindowSequenceData backData = backSequence.Peek();
                    if (backData.hideTargetWindow != null)
                    {
                        // 栈顶不是即将显示界面(导航序列被打断)
                        if (backData.hideTargetWindow.GetID != baseWindow.GetID)
                        {
                            Debuger.Log("[**Need to clear all back window sequence data**].");
                            Debuger.Log("[hide target window and show window id is " + backData.hideTargetWindow.GetID + " != " + baseWindow.GetID);
                            backSequence.Clear();
                        }
                        else
                        {
                            // NeedBack类型要将backShowTargets界面显示
                            if (windowData.showMode == UIWindowShowMode.NeedBack
                                && backData.backShowTargets != null)
                            {
                                    for (int i = 0; i < backData.backShowTargets.Count; i++)
                                    {
                                        WindowID backId = backData.backShowTargets[i];
                                        // 保证最上面为currentShownWindow
                                        if (i == backData.backShowTargets.Count - 1)
                                        {
                                            Debug.Log("change currentShownNormalWindow : " + backId);
                                            // 改变当前活跃Normal窗口
                                            this.lastShownNormalWindow = this.curShownNormalWindow;
                                            this.curShownNormalWindow = GetGameWindow(backId);                                            
                                        }
                                        ShowWindowForBack(backId);
                                    }
                            }
                        }
                    }
                    else
                        Debug.LogError("Back data hide target window is null!");
                }
            }
        }

        public Transform GetTargetRoot(UIWindowType type)
        {
            if (type == UIWindowType.Fixed)
                return UIFixedWidowRoot;
            if (type == UIWindowType.Normal)
                return UINormalWindowRoot;
            if (type == UIWindowType.PopUp)
                return UIPopUpWindowRoot;
            return UIRoot;
        }

        /// <summary>
        /// MessageBox
        /// </summary>
        /// 
        public void ShowMessageBox(string msg)
        {
            UIBaseWindow msgWindow = ReadyToShowBaseWindow(WindowID.WindowID_MessageBox);
            if (msgWindow != null)
            {
                ((UIMessageBox)msgWindow).SetMsg(msg);
                ((UIMessageBox)msgWindow).ResetWindow();
                RealShowWindow(msgWindow, WindowID.WindowID_MessageBox);
            }
        }

        public void ShowMessageBox(string msg, string centerStr, UIEventListener.VoidDelegate callBack)
        {
            UIBaseWindow msgWindow = ReadyToShowBaseWindow(WindowID.WindowID_MessageBox);
            if (msgWindow != null)
            {
                UIMessageBox messageBoxWindow = ((UIMessageBox)msgWindow);
                ((UIMessageBox)msgWindow).ResetWindow();
                messageBoxWindow.SetMsg(msg);
                messageBoxWindow.SetCenterBtnCallBack(centerStr, callBack);
                RealShowWindow(msgWindow, WindowID.WindowID_MessageBox);
            }
        }

        public void ShowMessageBox(string msg, string leftStr, UIEventListener.VoidDelegate leftCallBack, string rightStr, UIEventListener.VoidDelegate rightCallBack)
        {
            UIBaseWindow msgWindow = ReadyToShowBaseWindow(WindowID.WindowID_MessageBox);
            if (msgWindow != null)
            {
                UIMessageBox messageBoxWindow = ((UIMessageBox)msgWindow);
                ((UIMessageBox)msgWindow).ResetWindow();
                messageBoxWindow.SetMsg(msg);
                messageBoxWindow.SetRightBtnCallBack(rightStr, rightCallBack);
                messageBoxWindow.SetLeftBtnCallBack(leftStr, leftCallBack);
                RealShowWindow(msgWindow, WindowID.WindowID_MessageBox);
            }
        }

        public void CloseMessageBox(Action onClosed = null)
        {
            HideWindow(WindowID.WindowID_MessageBox);
        }
    }
}


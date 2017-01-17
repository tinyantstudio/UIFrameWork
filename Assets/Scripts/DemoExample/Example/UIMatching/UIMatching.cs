using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TinyFrameWork
{
    /// <summary>
    /// Matching Window
    /// </summary>
    public class UIMatching : UIWindowBase
    {
        private WindowID targetBackWindowId = WindowID.WindowID_LevelDetail;

        private GameObject btnWin;
        private GameObject btnLose;
        private GameObject btnShop;

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_Matching;
        }

        public override void InitWindowOnAwake()
        {
            base.InitWindowOnAwake();
            InitWindowCoreData();

            btnWin = GameUtility.FindDeepChild(this.gameObject, "BtnWin").gameObject;
            btnLose = GameUtility.FindDeepChild(this.gameObject, "BtnLose").gameObject;
            btnShop = GameUtility.FindDeepChild(this.gameObject, "BtnShop").gameObject;

            // win the game
            // load new scene to show target window
            UIEventListener.Get(btnWin).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", false, delegate
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_MatchResult);
                    UIWindowBase baseWindow = UICenterMasterManager.Instance.GetGameWindow(WindowID.WindowID_MatchResult);
                    ((UIMatchResult)baseWindow).SetMatchResult(true, targetBackWindowId);
                });
            };

            // lose the game
            // load new scene to show target window
            UIEventListener.Get(btnLose).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", false, delegate
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_MatchResult);
                    UIWindowBase baseWindow = UICenterMasterManager.Instance.GetGameWindow(WindowID.WindowID_MatchResult);
                    ((UIMatchResult)baseWindow).SetMatchResult(false, targetBackWindowId);
                });
            };

            // show navigation window shop
            UIEventListener.Get(btnShop).onClick = delegate
            {
                UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Shop);
            };
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.windowData.showMode = UIWindowShowMode.HideOtherWindow;
            this.windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        }

        public override void ShowWindow(BaseWindowContextData contextData = null)
        {
            base.ShowWindow(contextData);
            UICenterMasterManager.Instance.HideWindow(WindowID.WindowID_TopBar);
        }
        public void SetMatchingData(WindowID backIWindowId)
        {
            targetBackWindowId = backIWindowId;
        }
    }
}

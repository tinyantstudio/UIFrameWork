using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TinyFrameWork
{
    /// <summary>
    /// 比赛界面
    /// </summary>
    public class UIMatching : UIBaseWindow
    {
        private WindowID targetBackWindowId = WindowID.WindowID_LevelDetail;

        private GameObject btnWin;
        private GameObject btnLose;

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

            // win the game
            // load new scene to show target window
            UIEventListener.Get(btnWin).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", delegate
                {
                    // 是否需要一个退出比赛单独接口?
                    // UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
                    // UIManager.GetInstance().ShowWindow(WindowID.WindowID_LevelDetail);

                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_MatchResult);
                    UIBaseWindow baseWindow = UICenterMasterManager.Instance.GetGameWindow(WindowID.WindowID_MatchResult);
                    ((UIMatchResult)baseWindow).SetMatchResult(true, targetBackWindowId);
                });
            };

            // lose the game
            // load new scene to show target window
            UIEventListener.Get(btnLose).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", delegate
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_MatchResult);
                    UIBaseWindow baseWindow = UICenterMasterManager.Instance.GetGameWindow(WindowID.WindowID_MatchResult);
                    ((UIMatchResult)baseWindow).SetMatchResult(false, targetBackWindowId);
                });
            };
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.windowData.showMode = UIWindowShowMode.HideOtherWindow;
            this.windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        }

        public void SetMatchingData(WindowID backIWindowId)
        {
            targetBackWindowId = backIWindowId;
        }
    }
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    public class UIRankManager : UIManagerBase
    {
        private static UIRankManager instance;
        public static UIRankManager GetInstance()
        {
            return instance;
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }
        /// <summary>
        /// Init the rank sub window Manager
        /// 1. Find the managed sub window
        /// 2. init the managed sub window
        /// </summary>
        public override void InitWindowManager()
        {
            base.InitWindowManager();
            InitWindowControl();
            isNeedWaitHideOver = true;

            // UIRankDetail sub window
            GameObject objRankDetail = GameUtility.FindDeepChild(this.gameObject, "DetailWindowContainer").gameObject;
            UIRankDetail rankDetailScript = objRankDetail.GetComponent<UIRankDetail>();
            if (rankDetailScript == null)
                rankDetailScript = objRankDetail.AddComponent<UIRankDetail>();

            dicAllWindows[(int)WindowID.WindowID_Rank_Detail] = rankDetailScript;

            // UIRankOwnDetail sub window
            GameObject objRankOwnDetail = GameUtility.FindDeepChild(this.gameObject, "OwnDetailWindow").gameObject;
            UIRankOwnDetail rankOwnDetailScript = objRankOwnDetail.GetComponent<UIRankOwnDetail>();
            if (rankOwnDetailScript == null)
                rankOwnDetailScript = objRankOwnDetail.AddComponent<UIRankOwnDetail>();

            dicAllWindows[(int)WindowID.WindowID_Rank_OwnDetail] = rankOwnDetailScript;
        }

        protected override void InitWindowControl()
        {
            this.managedWindowIds.Clear();
            AddWindowInControl(WindowID.WindowID_Rank_Detail);
            AddWindowInControl(WindowID.WindowID_Rank_OwnDetail);
        }

        public override void ShowWindow(WindowID id, ShowWindowData showData)
        {
            if (!IsWindowInControl(id))
            {
                Debuger.Log("UIRankManager has no control power of " + id.ToString());
                return;
            }
            if (dicShownWindows.ContainsKey((int)id))
                return;
            if (dicAllWindows.ContainsKey((int)id))
            {
                UIBaseWindow baseWindow = dicAllWindows[(int)id];
                if (baseWindow.ID != id)
                {
                    Debuger.LogError(string.Format("[UIRankManager BaseWindowId :{0} != shownWindowId :{1}]", baseWindow.ID, id));
                    return;
                }
                if (baseWindow.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation)
                {
                    BackWindowSequenceData backData = new BackWindowSequenceData();
                    backData.hideTargetWindow = baseWindow;
                    backSequence.Push(backData);
                }
                this.RealShowWindow(baseWindow, baseWindow.ID, showData);
            }
        }
        protected override int GetCurrenShownWindow()
        {
            if (backSequence.Count > 0)
            {
                BackWindowSequenceData data = backSequence.Peek();
                return (int)data.hideTargetWindow.ID;
            }
            return (int)WindowID.WindowID_Invaild;
        }

        public override void HideWindow(WindowID id, System.Action onComplete)
        {
            CheckDirectlyHide(id, onComplete);
        }

        public override void ResetAllInControlWindows()
        {
            foreach (KeyValuePair<int, UIBaseWindow> childWindow in dicAllWindows)
            {
                childWindow.Value.ResetWindow();
            }
            dicShownWindows.Clear();
        }

        public override bool PopNavigationWindow()
        {
            bool isValidBack = RealPopNavigationWindow();
            return isValidBack;
        }
    }
}
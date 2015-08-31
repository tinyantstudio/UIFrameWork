using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoolGame
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
        /// 初始化RankWindow界面管理类
        /// 1.查找子界面
        /// 2.初始化子界面
        /// </summary>
        public override void InitWindowManager()
        {
            base.InitWindowManager();
            InitWindowControl();
            isNeedWaitHideOver = true;

            // UIRankDetail子界面
            GameObject objRankDetail = GameUtility.FindDeepChild(this.gameObject, "DetailWindowContainer").gameObject;
            UIRankDetail rankDetailScript = objRankDetail.GetComponent<UIRankDetail>();
            if (rankDetailScript == null)
                rankDetailScript = objRankDetail.AddComponent<UIRankDetail>();

            allWindows[WindowID.WindowID_Rank_Detail] = rankDetailScript;

            // UIRankOwnDetail子界面
            GameObject objRankOwnDetail = GameUtility.FindDeepChild(this.gameObject, "OwnDetailWindow").gameObject;
            UIRankOwnDetail rankOwnDetailScript = objRankOwnDetail.GetComponent<UIRankOwnDetail>();
            if (rankOwnDetailScript == null)
                rankOwnDetailScript = objRankOwnDetail.AddComponent<UIRankOwnDetail>();

            allWindows[WindowID.WindowID_Rank_OwnDetail] = rankOwnDetailScript;
        }

        protected override void InitWindowControl()
        {
            this.managedWindowId = 0;
            AddWindowInControl(WindowID.WindowID_Rank_Detail);
            AddWindowInControl(WindowID.WindowID_Rank_OwnDetail);
        }

        public override void ShowWindow(WindowID id, ShowWindowData data)
        {
            if (!IsWindowInControl(id))
            {
                Debug.Log("UIRankManager has no control power of " + id.ToString());
                return; 
            }
            if (shownWindows.ContainsKey(id))
                return;
            if (allWindows.ContainsKey(id))
            {
                UIBaseWindow baseWindow = allWindows[id];
                if (baseWindow.windowData.showMode == UIWindowShowMode.NeedBack)
                {
                    BackWindowSequenceData backData = new BackWindowSequenceData();
                    backData.hideTargetWindow = baseWindow;
                    backSequence.Push(backData);
                }
                allWindows[id].ShowWindow();
                shownWindows[id] = allWindows[id];

                this.lastShownNormalWindow = this.curShownNormalWindow;
                curShownNormalWindow = baseWindow;
            }
        }

        public override void HideWindow(WindowID id, System.Action onComplete)
        {
            CheckDirectlyHide(id, onComplete);
        }

        public override void ResetAllInControlWindows()
        {
            foreach (KeyValuePair<WindowID, UIBaseWindow> childWindow in allWindows)
            {
                childWindow.Value.ResetWindow(); 
            }
            shownWindows.Clear();
        }

        public override bool ReturnWindow()
        {
            bool isValidBack = RealReturnWindow();
            return isValidBack;
        }
    } 
}


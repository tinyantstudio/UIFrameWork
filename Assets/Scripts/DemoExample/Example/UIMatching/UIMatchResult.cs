using UnityEngine;
using System.Collections;

namespace TinyFrameWork
{
    public class UIMatchResult : UIBaseWindow
    {
        private WindowID targetBackWindowId = WindowID.WindowID_LevelDetail;

        private GameObject btnRank;
        private GameObject btnSkill;
        private GameObject btnLoseContine;
        private GameObject btnWinContine;

        private UILabel lbResultMsg;
        private UILabel lbResult;

        private GameObject gbLoseContent;
        private GameObject gbWinContent;

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_MatchResult;
        }

        public override void InitWindowOnAwake()
        {
            base.InitWindowOnAwake();
            InitWindowCoreData();

            gbWinContent = GameUtility.FindDeepChild(this.gameObject, "WinContent").gameObject;
            gbLoseContent = GameUtility.FindDeepChild(this.gameObject, "LoseContent").gameObject;

            btnRank = GameUtility.FindDeepChild(this.gameObject, "LoseContent/Btns/Btn_Rank").gameObject;
            btnSkill = GameUtility.FindDeepChild(this.gameObject, "LoseContent/Btns/Btn_Skill").gameObject;
            btnLoseContine = GameUtility.FindDeepChild(this.gameObject, "LoseContent/Btns/Btn_Continue").gameObject;
            btnWinContine = GameUtility.FindDeepChild(this.gameObject, "WinContent/Btn_Continue").gameObject;
            lbResult = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Msg/Sprite/Label");
            lbResultMsg = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Msg/Label");

            UIEventListener.Get(btnLoseContine).onClick = OnContineBtn;
            UIEventListener.Get(btnWinContine).onClick = OnContineBtn;

            UIEventListener.Get(btnSkill).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", true, delegate
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_TopBar);
                    ShowWindowData showData = new ShowWindowData();
                    showData.checkNavigation = true;
                    showData.ignoreAddNavData = true;
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Skill, showData);
                });
            };

            UIEventListener.Get(btnRank).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", true, delegate
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_TopBar);
                    ShowWindowData showData = new ShowWindowData();
                    showData.checkNavigation = true;
                    showData.ignoreAddNavData = true;
                    UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_LevelDetail, showData);
                });
            };
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.windowData.showMode = UIWindowShowMode.HideOtherWindow;
            this.windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            this.preWindowID = WindowID.WindowID_LevelDetail;
        }

        public void SetMatchResult(bool isWin, WindowID backWindowId)
        {
            targetBackWindowId = backWindowId;
            if (isWin)
            {
                NGUITools.SetActive(gbLoseContent, false);
                NGUITools.SetActive(gbWinContent, true);

                lbResultMsg.text = "Win the matching Feel Good.";
                lbResult.text = "WIN";
            }
            else
            {
                NGUITools.SetActive(gbLoseContent, true);
                NGUITools.SetActive(gbWinContent, false);

                lbResult.text = "LOSE";
                lbResultMsg.text = "Lose the matching Do better next Time.";
            }
        }

        private void OnContineBtn(GameObject obj)
        {
            GameMonoHelper.GetInstance().LoadGameScene("RealGame-EmptyScene", true, delegate
            {
                UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_TopBar);
                ShowWindowData showData = new ShowWindowData();
                showData.checkNavigation = true;
                showData.ignoreAddNavData = true;
                UICenterMasterManager.Instance.ShowWindow(targetBackWindowId, showData);
            });

        }
    }
}


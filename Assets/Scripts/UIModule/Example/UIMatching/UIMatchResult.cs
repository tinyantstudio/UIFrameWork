using UnityEngine;
using System.Collections;

namespace CoolGame
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

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_MatchResult;
            base.InitWindowOnAwake();
            InitWindowData();

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
                //ShowWindowData windowData = new ShowWindowData();
                //windowData.needClearBackSequence = true;
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_Skill);
            };

            UIEventListener.Get(btnRank).onClick = delegate
            {
                //UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
                //UIManager.GetInstance().ShowWindow(WindowID.WindowID_Rank);
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_Level);
            };
        }

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.showMode = UIWindowShowMode.NoNeedBack;
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
            // 返回到进入比赛界面
            // 比赛入口不同
            UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
            UIManager.GetInstance().ShowWindow(targetBackWindowId, null);
        }
    }
}


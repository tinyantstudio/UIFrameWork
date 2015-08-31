using UnityEngine;
using System.Collections;

namespace CoolGame
{
    public class UIMainMenu : UIBaseWindow
    {
        private GameObject btnInfor;
        private GameObject btnRank;
        private GameObject btnLevel;
        private GameObject btnSkill;
        private GameObject btnSwitch;


        private UILabel lbSwitch;

        private TweenPosition[] twPositions;
        private bool switchFlag = true;
        public override void ShowWindow()
        {
            this.gameObject.SetActive(true);
        }

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_MainMenu; 
            base.InitWindowOnAwake();
            InitWindowData();

            twPositions = new TweenPosition[4];
            btnInfor = GameUtility.FindDeepChild(this.gameObject, "LeftBottom/Btns/Btn_Infor").gameObject;
            btnRank = GameUtility.FindDeepChild(this.gameObject, "LeftBottom/Btns/Btn_Rank").gameObject;
            btnLevel = GameUtility.FindDeepChild(this.gameObject, "LeftBottom/Btns/Btn_Level").gameObject;
            btnSwitch = GameUtility.FindDeepChild(this.gameObject, "BottomRight/Btn_Switch").gameObject;
            btnSkill = GameUtility.FindDeepChild(this.gameObject, "LeftBottom/Btns/Btn_Skill").gameObject;
            lbSwitch = GameUtility.FindDeepChild<UILabel>(this.gameObject, "BottomRight/Btn_Switch/Label");
            
            twPositions[0] = btnInfor.GetComponent<TweenPosition>();
            twPositions[1] = btnRank.GetComponent<TweenPosition>();
            twPositions[2] = btnLevel.GetComponent<TweenPosition>();
            twPositions[3] = btnSkill.GetComponent<TweenPosition>();

            UIEventListener.Get(btnRank).onClick = delegate
            {
                ShowWindowData showData = new ShowWindowData();
                showData.forceResetWindow = true;
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_Rank, showData);
            };

            UIEventListener.Get(btnSwitch).onClick = delegate
            {
                switchFlag = !switchFlag;
                if (switchFlag)
                    ShowBtns();
                else
                    HideBtns();
            };

            UIEventListener.Get(btnLevel).onClick = delegate
            {
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_Level);
            };

            UIEventListener.Get(btnSkill).onClick = delegate
            {
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_Skill);
            };
        }

        public void ShowBtns()
        {
            lbSwitch.text = "On";
            for (int i = 0; i < twPositions.Length; i++)
                twPositions[i].PlayReverse();
        }
        public void HideBtns()
        {
            lbSwitch.text = "Off";
            for (int i = 0; i < twPositions.Length;i++ )
                twPositions[i].PlayForward();
        }

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.isStartWindow = true;
        }
    } 
}


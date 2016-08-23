using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
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
        public override void ShowWindow(BaseWindowContextData contextData)
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

                string[] headIcons = new string[] { "Rambo", "Angry", "Smile", "Laugh", "Dead", "Frown", "Annoyed" };

                // fill the rank data
                ContextDataRank contextData = new ContextDataRank();
                contextData.listRankItemDatas = new List<RankItemData>();

                // rank list
                for (int i = 0; i < 10; i++)
                {
                    RankItemData newData = new RankItemData();
                    newData.playerName = headIcons[UnityEngine.Random.Range(0, headIcons.Length)];
                    newData.headIcon = "Emoticon - " + newData.playerName;
                    newData.playerName = "Mr." + newData.playerName;
                    newData.playerDescription = string.Format("I'm {0}", newData.playerName);
                    contextData.listRankItemDatas.Add(newData);
                }
                showData.contextData = contextData;
                UICenterMasterManager.GetInstance().ShowWindow(WindowID.WindowID_Rank, showData);
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
                UICenterMasterManager.GetInstance().ShowWindow(WindowID.WindowID_Level);
            };

            UIEventListener.Get(btnSkill).onClick = delegate
            {
                UICenterMasterManager.GetInstance().ShowWindow(WindowID.WindowID_Skill);
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
            for (int i = 0; i < twPositions.Length; i++)
                twPositions[i].PlayForward();
        }

        protected override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.forceClearNavigation = true;
        }
    }
}


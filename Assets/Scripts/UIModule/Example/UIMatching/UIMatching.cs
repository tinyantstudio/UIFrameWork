using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CoolGame
{
    /// <summary>
    /// 比赛界面
    /// </summary>
    public class UIMatching : UIBaseWindow
    {
        private WindowID targetBackWindowId = WindowID.WindowID_LevelDetail;

        private GameObject btnWin;
        private GameObject btnLose;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_Matching;
            base.InitWindowOnAwake();

            InitWindowData();

            btnWin = GameUtility.FindDeepChild(this.gameObject, "BtnWin").gameObject;
            btnLose = GameUtility.FindDeepChild(this.gameObject, "BtnLose").gameObject;

            // win the game
            UIEventListener.Get(btnWin).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("TestEmptyScene", delegate
                {
                    // 是否需要一个退出比赛单独接口?
                    // UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
                    // UIManager.GetInstance().ShowWindow(WindowID.WindowID_LevelDetail);

                    UIManager.GetInstance().ShowWindow(WindowID.WindowID_MatchResult);
                    UIBaseWindow baseWindow = UIManager.GetInstance().GetGameWindow(WindowID.WindowID_MatchResult);
                    ((UIMatchResult)baseWindow).SetMatchResult(true, targetBackWindowId);
                });
            };

            // lose the game
            UIEventListener.Get(btnLose).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("TestEmptyScene", delegate
                {
                    UIManager.GetInstance().ShowWindow(WindowID.WindowID_MatchResult);
                    UIBaseWindow baseWindow = UIManager.GetInstance().GetGameWindow(WindowID.WindowID_MatchResult);
                    ((UIMatchResult)baseWindow).SetMatchResult(false, targetBackWindowId);
                });
            };
        }

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.showMode = UIWindowShowMode.NoNeedBack;
        }

        public void SetMatchingData(WindowID backIWindowId)
        {
            targetBackWindowId = backIWindowId;
        }
    }
}


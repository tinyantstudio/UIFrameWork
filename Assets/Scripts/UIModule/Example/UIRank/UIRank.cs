using UnityEngine;
using System.Collections;
using System;

namespace CoolGame
{
    /// <summary>
    /// 排行榜界面
    /// </summary>
    public class UIRank : UIBaseWindow, IWindowAnimation
    {
        private Transform objEffect;
        private UIRankManager rankWindowManager;
        private Transform itemsGrid;
        private UIScrollView scrollView;

        private GameObject btnGoToMatch;

        // aniamtion content
        private Transform trsAnimation;
        private TweenPosition twPosition;

        // test prefab Item
        public GameObject itemTemplate;
        private string[] headIcons = new string[] { "Rambo", "Angry", "Smile", "Laugh", "Dead", "Frown", "Annoyed" };

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_Rank;
            this.preWindowID = WindowID.WindowID_MainMenu;
            InitWindowData();
            base.InitWindowOnAwake();

            // this.RegisterReturnLogic(RetrunPreLogic);

            objEffect = GameUtility.FindDeepChild(this.gameObject, "EffectParent");
            itemsGrid = GameUtility.FindDeepChild(this.gameObject, "LeftAnchor/LeftMain/Scroll View/Grid");
            trsAnimation = GameUtility.FindDeepChild(this.gameObject, "LeftAnchor/LeftMain");
            scrollView = GameUtility.FindDeepChild<UIScrollView>(this.gameObject, "LeftAnchor/LeftMain/Scroll View");
            btnGoToMatch = GameUtility.FindDeepChild(this.gameObject, "Btn_GotoMatch").gameObject;
            
            rankWindowManager = this.gameObject.GetComponent<UIRankManager>();
            if (rankWindowManager == null)
            {
                rankWindowManager = this.gameObject.AddComponent<UIRankManager>();
                rankWindowManager.InitWindowManager();
            }

            UIEventListener.Get(btnGoToMatch).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("AnimationCurve",
                    delegate
                    {
                        UIManager.GetInstance().ShowWindow(WindowID.WindowID_Matching);
                        UIManager.GetInstance().GetGameWindowScript<UIMatching>(WindowID.WindowID_Matching).SetMatchingData(this.windowID);
                    });
            };
        }

        public override void ResetWindow()
        {
            scrollView.ResetPosition();
            GetWindowManager.ResetAllInControlWindows();
            ResetAnimation();
        }

        public override void InitWindowData()
        {
            base.InitWindowData();

            windowData.showMode = UIWindowShowMode.HideOther;
            windowData.windowType = UIWindowType.Normal;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
        }

        public override void ShowWindow()
        {
            NGUITools.SetActive(this.gameObject, true);
            // 显示主界面，选择性显示子界面
            // UIRankManager.GetInstance().ShowWindow(WindowID.WindowID_Rank_OwnDetail);
            EnterAnimation(delegate
            {
                Debug.Log("UIRank window's enter animation is over.");
            });

            // test
            FillRankItem();
        }

        public override void HideWindow(Action onComplete)
        {
            // 隐藏主界面，可以选择性隐藏子界面
            UIRankManager.GetInstance().HideWindow(WindowID.WindowID_Rank_OwnDetail, null);
            QuitAnimation(delegate
            {
                Debug.Log("UIRank window's Hide animation is over");
                NGUITools.SetActive(this.gameObject, false);
                if (onComplete != null)
                    onComplete();
            });
        }

        public void EnterAnimation(EventDelegate.Callback onComplete)
        {
            if (twPosition == null)
                twPosition = trsAnimation.GetComponent<TweenPosition>();
            twPosition.PlayForward();
            EventDelegate.Set(twPosition.onFinished, onComplete);
        }

        public void QuitAnimation(EventDelegate.Callback onComplete)
        {
            if (twPosition == null)
                twPosition = trsAnimation.GetComponent<TweenPosition>();
            twPosition.PlayReverse();
            EventDelegate.Set(twPosition.onFinished, onComplete);
        }

        public void ResetAnimation()
        {
            if (twPosition != null)
                twPosition.ResetToBeginningExtension(0.0f);
        }

        private void FillRankItem()
        {
            StartCoroutine(_FileRankItem());
        }

        IEnumerator _FileRankItem()
        {
            // Destroy all items
            for (int i = 0; i < itemsGrid.childCount; i++)
            {
                GameObject.Destroy(itemsGrid.GetChild(i).gameObject);
            }
            yield return new WaitForEndOfFrame();
            itemsGrid.GetComponent<UIGrid>().Reposition();

            // fill items by given data
            for (int i = 0; i < 10; i++)
            {
                GameObject item = NGUITools.AddChild(itemsGrid.gameObject, itemTemplate);
                UIRankItem itemScript = item.GetComponent<UIRankItem>();

                string playerName = headIcons[UnityEngine.Random.Range(0, headIcons.Length)];
                string headIcon = "Emoticon - " + playerName;
                itemScript.InitItem("Mr." + playerName, headIcon);
            }
            itemsGrid.GetComponent<UIGrid>().Reposition();
        }
    }
}


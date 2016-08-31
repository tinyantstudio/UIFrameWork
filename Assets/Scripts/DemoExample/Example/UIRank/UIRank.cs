using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace TinyFrameWork
{
    /// <summary>
    /// UIRank window 
    /// </summary>
    public class UIRank : UIBaseWindow, IWindowAnimation
    {
        private UIRankManager rankWindowManager;
        private Transform itemsGrid;
        private UIScrollView scrollView;

        private GameObject btnGoToMatch;

        // animation content
        private Transform trsAnimation;
        private TweenPosition twPosition;

        // test prefab Item
        public GameObject itemTemplate;

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_Rank;
        }

        public override void InitWindowOnAwake()
        {
            InitWindowCoreData();
            base.InitWindowOnAwake();
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

            // Just go to Game scene logic
            UIEventListener.Get(btnGoToMatch).onClick = delegate
            {
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-AnimationCurve",
                    delegate
                    {
                        UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Matching);
                        UICenterMasterManager.Instance.GetGameWindowScript<UIMatching>(WindowID.WindowID_Matching).SetMatchingData(this.ID);
                    });
            };
        }

        public override void ResetWindow()
        {
            scrollView.ResetPosition();
            GetWindowManager.ResetAllInControlWindows();
            ResetAnimation();
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.preWindowID = WindowID.WindowID_MainMenu;
            windowData.windowType = UIWindowType.Normal;
            windowData.showMode = UIWindowShowMode.HideOtherWindow;
            windowData.navigationMode = UIWindowNavigationMode.NeedAdded;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
        }

        public override void ShowWindow(BaseWindowContextData rankContextData)
        {
            NGUITools.SetActive(this.gameObject, true);
            // you can also use Rank's window manager to show target child window
            // UIRankManager.GetInstance().ShowWindow(WindowID.WindowID_Rank_OwnDetail);
            EnterAnimation(delegate
            {
                Debuger.Log("## UIRank window's enter animation is over.");
            });

            ContextDataRank data = rankContextData as ContextDataRank;
            if (data != null)
                FillRankItem(data.listRankItemDatas);
        }

        public override void HideWindow(Action onComplete)
        {
            // Hide target child window
            UIRankManager.GetInstance().HideWindow(WindowID.WindowID_Rank_OwnDetail, null);
            QuitAnimation(delegate
            {
                Debuger.Log("## UIRank window's Hide animation is over");
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

        private void FillRankItem(List<RankItemData> listData)
        {
            StartCoroutine(_FileRankItem(listData));
        }

        IEnumerator _FileRankItem(List<RankItemData> listData)
        {
            // Destroy all items
            for (int i = 0; i < itemsGrid.childCount; i++)
            {
                GameObject.Destroy(itemsGrid.GetChild(i).gameObject);
            }
            yield return new WaitForEndOfFrame();
            itemsGrid.GetComponent<UIGrid>().Reposition();

            // fill items by context rank data
            for (int i = 0; i < listData.Count; i++)
            {
                GameObject item = NGUITools.AddChild(itemsGrid.gameObject, itemTemplate);
                UIRankItem itemScript = item.GetComponent<UIRankItem>();
                RankItemData itemData = listData[i];
                string playerName = itemData.playerName;
                string headIcon = itemData.headIcon;
                itemScript.InitItem(playerName, headIcon);
            }
            itemsGrid.GetComponent<UIGrid>().Reposition();
        }
    }
}


using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    public class UIRankOwnDetail : UIBaseWindow, IWindowAnimation
    {
        private GameObject btnClose;
        private UISprite spHeadIcon;

        // animation content
        private GameObject objAnimation;
        private TweenPosition twPosition;

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_Rank_OwnDetail; 
        }

        public override void InitWindowOnAwake()
        {
            InitWindowCoreData();
            base.InitWindowOnAwake();

            spHeadIcon = GameUtility.FindDeepChild<UISprite>(this.gameObject, "Sprite");
            btnClose = GameUtility.FindDeepChild(this.gameObject, "Close").gameObject;
            objAnimation = this.gameObject;

            UIEventListener.Get(btnClose).onClick = OnBtnSwitch;
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.windowData.navigationMode = UIWindowNavigationMode.NormalNavigation;
        }

        public override void ResetWindow()
        {
            isShown = false;
            ResetAnimation();
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            isShown = true;
            EnterAnimation(delegate 
            {
                TweenColor.Begin(spHeadIcon.gameObject, 0.35f, new Color(1.0f, 1.0f, 1.0f));
            });
        }

        public override void HideWindow(System.Action onComplete)
        {
            isShown = false;
            QuitAnimation(delegate
            {
                if (onComplete != null)
                    onComplete();
            });
        }

        public void EnterAnimation(EventDelegate.Callback onComplete)
        {
            if (twPosition == null)
                twPosition = objAnimation.GetComponent<TweenPosition>();
            twPosition.PlayForward();
            EventDelegate.Set(twPosition.onFinished, onComplete);
        }

        public void QuitAnimation(EventDelegate.Callback onComplete)
        {
            if (twPosition == null)
                twPosition = objAnimation.GetComponent<TweenPosition>();
            twPosition.PlayReverse();
            EventDelegate.Set(twPosition.onFinished, onComplete);
        }

        public void ResetAnimation()
        {
            if (twPosition != null)
                twPosition.ResetToBeginningExtension(0.0f);
        }

        public void UpdateOwnDetail()
        {
        }

        private void OnBtnSwitch(GameObject obj)
        {
            if (base.isShown)
                UIRankManager.GetInstance().HideWindow(this.ID, null);
            else
                UIRankManager.GetInstance().ShowWindow(this.ID, null);
        }
    }
}


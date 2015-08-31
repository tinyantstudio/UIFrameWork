using UnityEngine;
using System.Collections;

namespace CoolGame
{
    public class UIRankOwnDetail : UIBaseWindow, IWindowAnimation
    {
        private UILabel lbName;
        private GameObject btnClose;
        private UISprite spHeadIcon;

        // animation content
        private GameObject objAnimation;
        private TweenPosition twPosition;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_Rank_OwnDetail;
            InitWindowData();
            base.InitWindowOnAwake();

            lbName = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Name");
            spHeadIcon = GameUtility.FindDeepChild<UISprite>(this.gameObject, "Sprite");
            btnClose = GameUtility.FindDeepChild(this.gameObject, "Close").gameObject;
            objAnimation = this.gameObject;

            UIEventListener.Get(btnClose).onClick = OnBtnSwitch;
        }

        public override void ResetWindow()
        {
            isShown = false;
            ResetAnimation();
        }

        public override void ShowWindow()
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
                UIRankManager.GetInstance().HideWindow(windowID, null);
            else
                UIRankManager.GetInstance().ShowWindow(windowID, null);
        }

    } 
}


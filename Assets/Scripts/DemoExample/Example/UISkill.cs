using UnityEngine;
using System.Collections;
using System;
namespace TinyFrameWork
{
    public class UISkill : UIWindowBase, IWindowAnimation
    {
        private GameObject gbAnimation;
        private GameObject btnGoToLevel;
        private TweenPosition twPosition;

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_Skill;
        }

        public override void InitWindowOnAwake()
        {
            base.InitWindowOnAwake();
            InitWindowCoreData();

            gbAnimation = GameUtility.FindDeepChild(this.gameObject, "Center/Sprite").gameObject;
            btnGoToLevel = GameUtility.FindDeepChild(this.gameObject, "RightAnchor/BtnLevel").gameObject;

            twPosition = gbAnimation.GetComponent<TweenPosition>();

            UIEventListener.Get(btnGoToLevel).onClick = delegate
            {
                UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Level);
            };
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.preWindowID = WindowID.WindowID_MainMenu;
            this.windowData.showMode = UIWindowShowMode.HideOtherWindow;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
            this.windowData.navigationMode = UIWindowNavigationMode.NormalNavigation;
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            NGUITools.SetActive(this.gameObject, true);
            IsLock = true;
            EnterAnimation(delegate
            {
                IsLock = false;
            });
        }

        public override void HideWindow(Action onComplete)
        {
            IsLock = true;
            QuitAnimation(delegate
            {
                NGUITools.SetActive(this.gameObject, false);
                if (onComplete != null)
                    onComplete();
            });
        }

        public void EnterAnimation(EventDelegate.Callback callBack)
        {
            twPosition.PlayForward();
            EventDelegate.Set(twPosition.onFinished, callBack);
        }

        public void QuitAnimation(EventDelegate.Callback callBack)
        {
            twPosition.PlayReverse();
            EventDelegate.Set(twPosition.onFinished, callBack);
        }

        public override void ResetWindow()
        {
            base.ResetWindow();
            ResetAnimation();
        }

        public void ResetAnimation()
        {
            twPosition.ResetToBeginningExtension(0.0f);
        }
    }
}


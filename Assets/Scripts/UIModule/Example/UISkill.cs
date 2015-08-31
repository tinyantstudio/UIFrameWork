using UnityEngine;
using System.Collections;
using System;
namespace CoolGame
{
    public class UISkill : UIBaseWindow, IWindowAnimation
    {
        private GameObject gbAnimation;
        private GameObject btnGoToLevel;
        private TweenPosition twPosition;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_Skill;
            this.preWindowID = WindowID.WindowID_MainMenu;

            base.InitWindowOnAwake();
            InitWindowData();

            gbAnimation = GameUtility.FindDeepChild(this.gameObject, "Center/Sprite").gameObject;
            btnGoToLevel = GameUtility.FindDeepChild(this.gameObject, "RightAnchor/BtnLevel").gameObject;

            twPosition = gbAnimation.GetComponent<TweenPosition>();

            UIEventListener.Get(btnGoToLevel).onClick = delegate
            {
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_Level);
            };
        }

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.showMode = UIWindowShowMode.HideOther;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
        }

        public override void ShowWindow()
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


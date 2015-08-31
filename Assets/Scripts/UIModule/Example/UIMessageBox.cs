using UnityEngine;
using System;
using System.Collections;

namespace CoolGame
{
    public class UIMessageBox : UIBaseWindow, IWindowAnimation
    {
        private UILabel lbMsg;
        private UILabel lbCenter;
        private UILabel lbRight;
        private UILabel lbLeft;

        private GameObject btnLeft;
        private GameObject btnRight;
        private GameObject btnCenter;
        private GameObject btnClose;
        private GameObject objAnimation;

        private TweenScale twScale;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_MessageBox;
            InitWindowData();
            base.InitWindowOnAwake();

            lbMsg = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Message");
            btnCenter = GameUtility.FindDeepChild(this.gameObject, "Btns/CenterBtn").gameObject;
            btnRight = GameUtility.FindDeepChild(this.gameObject, "Btns/RightBtn").gameObject;
            btnLeft = GameUtility.FindDeepChild(this.gameObject, "Btns/LeftBtn").gameObject;
            btnClose = GameUtility.FindDeepChild(this.gameObject, "Btns/Close").gameObject;
            objAnimation = GameUtility.FindDeepChild(this.gameObject, "Center").gameObject;
            lbCenter = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Btns/CenterBtn/Label");
            lbLeft = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Btns/LeftBtn/Label");
            lbRight = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Btns/RightBtn/Label");

            UIEventListener.Get(btnClose).onClick = delegate
            {
                UIManager.GetInstance().CloseMessageBox();
            };
        }

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.windowType = UIWindowType.PopUp;
            this.windowData.showMode = UIWindowShowMode.DoNothing;
            this.windowData.colliderMode = UIWindowColliderMode.WithBg;
        }

        public override void ResetWindow()
        {
            base.ResetWindow();

            // 隐藏所有按钮
            NGUITools.SetActive(btnCenter, false);
            NGUITools.SetActive(btnLeft, false);
            NGUITools.SetActive(btnRight, false);
        }

        public void SetMsg(string msg)
        {
            lbMsg.text = msg;
        }

        public void SetCenterBtnCallBack(string msg, UIEventListener.VoidDelegate callBack)
        {
            lbCenter.text = msg;
            NGUITools.SetActive(btnCenter, true);
            UIEventListener.Get(btnCenter).onClick = callBack;
        }

        public void SetLeftBtnCallBack(string msg, UIEventListener.VoidDelegate callBack)
        {
            lbLeft.text = msg;
            NGUITools.SetActive(btnLeft, true);
            UIEventListener.Get(btnLeft).onClick = callBack;
        }

        public void SetRightBtnCallBack(string msg, UIEventListener.VoidDelegate callBack)
        {
            lbRight.text = msg;
            NGUITools.SetActive(btnRight, true);
            UIEventListener.Get(btnRight).onClick = callBack;
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
                IsLock = false;
                NGUITools.SetActive(this.gameObject, false);
                if (onComplete != null)
                    onComplete();
            });
        }

        public void EnterAnimation(EventDelegate.Callback onComplete)
        {
            if (twScale == null)
                twScale = objAnimation.GetComponent<TweenScale>();
            twScale.PlayForward();
            EventDelegate.Set(twScale.onFinished, onComplete);
        }

        public void QuitAnimation(EventDelegate.Callback onComplete)
        {
            if (twScale == null)
                twScale = objAnimation.GetComponent<TweenScale>();
            twScale.PlayReverse();
            EventDelegate.Set(twScale.onFinished, onComplete);
        }

        public void ResetAnimation()
        {
            if (twScale != null)
                twScale.ResetToBeginningExtension(0.0f);
        }
    }
}


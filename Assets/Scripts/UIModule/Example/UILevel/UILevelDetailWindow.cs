using UnityEngine;
using System.Collections;

namespace CoolGame
{
    public class UILevelDetailWindow : UIBaseWindow, IWindowAnimation
    {
        private GameObject btnStart;
        private TweenAlpha twAlpha;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_LevelDetail;
            base.InitWindowOnAwake();
            InitWindowData();

            btnStart = GameUtility.FindDeepChild(this.gameObject, "LevelDetailRight/BtnEnter").gameObject;
            twAlpha = this.gameObject.GetComponent<TweenAlpha>();

            UIEventListener.Get(btnStart).onClick = delegate
            {
                // Application.LoadLevel("AnimationCurve");
                GameMonoHelper.GetInstance().LoadGameScene("AnimationCurve",
                    delegate
                    {
                        UIManager.GetInstance().ShowWindow(WindowID.WindowID_Matching);
                        UIManager.GetInstance().GetGameWindowScript<UIMatching>(WindowID.WindowID_Matching).SetMatchingData(this.windowID);
                    });
            };
        }

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
            this.windowData.showMode = UIWindowShowMode.HideOther;
        }

        public override void ShowWindow()
        {
            ResetAnimation();
            base.ShowWindow();
            IsLock = true;
            EnterAnimation(delegate
            {
                IsLock = false;
            });
        }

        public override void HideWindow(System.Action onCompleteHide = null)
        {
            IsLock = true;
            QuitAnimation(delegate
            {
                NGUITools.SetActive(this.gameObject, false);
                if (onCompleteHide != null)
                    onCompleteHide();
            });
        }

        public void EnterAnimation(EventDelegate.Callback onComplete)
        {
            if (twAlpha != null)
            {
                twAlpha.PlayForward();
                EventDelegate.Set(twAlpha.onFinished, onComplete);
            }
        }

        public void QuitAnimation(EventDelegate.Callback onComplete)
        {
            if (twAlpha != null)
            {
                twAlpha.PlayReverse();
                EventDelegate.Set(twAlpha.onFinished, onComplete);
            }
        }

        public override void ResetWindow()
        {
            base.ResetWindow();
            ResetAnimation();
        }

        public void ResetAnimation()
        {
            if (twAlpha != null)
                twAlpha.ResetToBeginningExtension(0.0f);
        }
    }
}

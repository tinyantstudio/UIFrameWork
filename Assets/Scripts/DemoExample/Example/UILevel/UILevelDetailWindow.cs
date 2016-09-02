using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TinyFrameWork
{
    /// <summary>
    /// Demo level detail window to show the Level Detail
    /// </summary>
    public class UILevelDetailWindow : UIBaseWindow, IWindowAnimation
    {
        private GameObject btnStart;
        private TweenAlpha twAlpha;

        private UILabel lbLevelDes;
        private UILabel lbLevelName;

        private List<GameObject> listStarts = new List<GameObject>();

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_LevelDetail;
        }

        public override void InitWindowOnAwake()
        {
            base.InitWindowOnAwake();
            InitWindowCoreData();

            btnStart = GameUtility.FindDeepChild(this.gameObject, "LevelDetailRight/BtnEnter").gameObject;
            this.lbLevelDes = GameUtility.FindDeepChild<UILabel>(this.gameObject, "LevelDetailRight/Content/des");
            this.lbLevelName = GameUtility.FindDeepChild<UILabel>(this.gameObject, "levelName");

            this.listStarts.Add(GameUtility.FindDeepChild(this.gameObject, "Stars/star01").gameObject);
            this.listStarts.Add(GameUtility.FindDeepChild(this.gameObject, "Stars/star02").gameObject);
            this.listStarts.Add(GameUtility.FindDeepChild(this.gameObject, "Stars/star03").gameObject);

            twAlpha = this.gameObject.GetComponent<TweenAlpha>();

            UIEventListener.Get(btnStart).onClick = delegate
            {
                // Application.LoadLevel("AnimationCurve");
                GameMonoHelper.GetInstance().LoadGameScene("RealGame-AnimationCurve",
                    delegate
                    {
                        UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Matching);
                        UICenterMasterManager.Instance.GetGameWindowScript<UIMatching>(WindowID.WindowID_Matching).SetMatchingData(this.ID);
                    });
            };
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.preWindowID = WindowID.WindowID_Level;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
            this.windowData.showMode = UIWindowShowMode.DoNothing;
            this.windowData.navigationMode = UIWindowNavigationMode.NormalNavigation;
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            ResetAnimation();
            base.ShowWindow(contextData);
            IsLock = true;
            EnterAnimation(delegate
            {
                IsLock = false;
            });

            ContextDataLevelDetail detail = contextData as ContextDataLevelDetail;
            if (detail != null)
            {
                this.lbLevelDes.text = detail.levelDescription;
                this.lbLevelName.text = detail.levelName;

                for (int i = 0; i < listStarts.Count; i++)
                    this.listStarts[i].SetActive(i + 1 <= detail.starCount);
            }
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
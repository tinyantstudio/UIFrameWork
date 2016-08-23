using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    /// <summary>
    /// Level system window
    /// </summary>
    public class UILevelWindow : UIBaseWindow, IWindowAnimation
    {
        public GameObject levelItem;
        private Transform trsLevelItemsParent;
        private TweenAlpha twAlpha;

        private bool realReturnToMainMenu = false;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_Level;
            InitWindowData();
            base.InitWindowOnAwake();
            // this.RegisterReturnLogic(RetrunPreLogic);
            trsLevelItemsParent = GameUtility.FindDeepChild(this.gameObject, "LevelItems/Items");
            twAlpha = gameObject.GetComponent<TweenAlpha>();
        }

        protected override void InitWindowData()
        {
            base.InitWindowData();
            this.preWindowID = WindowID.WindowID_MainMenu;
            // this.windowData.showMode = UIWindowShowMode.HideOther;
            this.windowData.showMode = UIWindowShowMode.HideOtherWindow;
            this.windowData.navigationMode = UIWindowNavigationMode.NeedAdded;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
        }

        public override void ShowWindow(BaseWindowContextData levelContextData)
        {
            realReturnToMainMenu = false;
            ResetAnimation();
            IsLock = true;
            NGUITools.SetActive(this.gameObject, true);
            FillLevelItems();

            EnterAnimation(delegate
            {
                IsLock = false;
            });
        }

        private List<string> levelNames = new List<string>() { "SkyBattle", "SkyCool", "SkyWorld", "SpaceWar", "ComeOn", "HellFight", "NewBattle", "King" };
        /// <summary>
        /// Test fill level items
        /// </summary>
        private void FillLevelItems()
        {
            int totalItemsCount = trsLevelItemsParent.childCount;
            for (int i = 0; i < totalItemsCount; i++)
            {
                Transform trs = trsLevelItemsParent.GetChild(i);

                if (trs.childCount > 0)
                    continue;
                GameObject item = NGUITools.AddChild(trs.gameObject, levelItem);
                UILevelItem itemScript = item.GetComponent<UILevelItem>();
                // itemScript.SetData("Level" + trs.name);
                itemScript.SetData(this.levelNames[i], Random.Range(0, 4));
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


        private bool RetrunPreLogic()
        {
            if (!realReturnToMainMenu)
            {
                UICenterMasterManager.GetInstance().ShowMessageBox(
                    "Do you want to leave level window?",
                    "Yes",
                    delegate
                    {
                        Debug.Log("Message Box click YES to leave level window.");
                        UICenterMasterManager.GetInstance().CloseMessageBox();
                        realReturnToMainMenu = true;
                        UICenterMasterManager.GetInstance().ReturnWindow();
                    },
                    "No",
                    delegate
                    {
                        Debug.Log("Message Box click NO.");
                        UICenterMasterManager.GetInstance().CloseMessageBox();
                    });
            }
            return !realReturnToMainMenu;
        }
    }
}

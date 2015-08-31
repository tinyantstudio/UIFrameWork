using UnityEngine;
using System.Collections;

namespace CoolGame
{
    /// <summary>
    /// 关卡界面
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

        public override void InitWindowData()
        {
            base.InitWindowData();
            this.preWindowID = WindowID.WindowID_MainMenu;
            this.windowData.showMode = UIWindowShowMode.HideOther;
            this.windowData.colliderMode = UIWindowColliderMode.Normal;
        }

        public override void ShowWindow()
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

        /// <summary>
        /// Test fill level items
        /// </summary>
        private void FillLevelItems()
        {
            int totalItemsCount = trsLevelItemsParent.childCount;
            for (int i = 0; i < totalItemsCount;i++ )
            {
                Transform trs = trsLevelItemsParent.GetChild(i);

                if (trs.childCount > 0)
                    continue;
                GameObject item = NGUITools.AddChild(trs.gameObject, levelItem);
                UILevelItem itemScript = item.GetComponent<UILevelItem>();
                itemScript.SetData("Level" + trs.name);
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
                UIManager.GetInstance().ShowMessageBox(
                    "Do you want to leave level window?",
                    "Yes",
                    delegate
                    {
                        Debug.Log("Message Box click YES to leave level window.");
                        UIManager.GetInstance().CloseMessageBox();
                        realReturnToMainMenu = true;
                        UIManager.GetInstance().ReturnWindow();
                    },
                    "No",
                    delegate
                    {
                        Debug.Log("Message Box click NO.");
                        UIManager.GetInstance().CloseMessageBox();
                    });
            }
            return !realReturnToMainMenu;
        }
    }
}

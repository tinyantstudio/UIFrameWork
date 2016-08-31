using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    public class UITopBar : UIBaseWindow
    {
        private GameObject btnReturn;
        private GameObject btnShowMsg;

        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_TopBar;
        }

        public override void InitWindowOnAwake()
        {
            this.windowData.windowType = UIWindowType.Fixed;

            base.InitWindowOnAwake();
            btnReturn = GameUtility.FindDeepChild(this.gameObject, "TopLeft/Btn_Rtn").gameObject;
            btnShowMsg = GameUtility.FindDeepChild(this.gameObject, "TopRight/Meg/Sprite").gameObject;

            UIEventListener.Get(btnReturn).onClick = delegate
            {
                UICenterMasterManager.Instance.ReturnWindow();
            };

            // message box Test.
            UIEventListener.Get(btnShowMsg).onClick = delegate
            {
                UICenterMasterManager.Instance.ShowMessageBox(
                    "You are yourself, Just Don't lose confidence.",
                    "Sure!",
                    delegate
                    {
                        UICenterMasterManager.Instance.CloseMessageBox();
                    });
                // UICenterMasterManager.Instance.ShowMessageBox("Hello World!");
            };
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            this.gameObject.SetActive(true);
        }

        void Update()
        {
            // test for PopWindow
            if (Input.GetKeyDown(KeyCode.P))
                UICenterMasterManager.Instance.ShowMessageBox("Oh press P show me!");
            else if (Input.GetKeyDown(KeyCode.S))
            {
                UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Shop);
                Debuger.Log("## show the pop window shop ##");
            }
        }
    }
}
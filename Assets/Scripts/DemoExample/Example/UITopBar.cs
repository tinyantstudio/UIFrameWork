using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    public class UITopBar : UIBaseWindow
    {
        private GameObject btnReturn;
        private GameObject btnShowMsg;
        private GameObject btnShop;

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
            btnShop = GameUtility.FindDeepChild(this.gameObject, "TopRight/BtnShop").gameObject;
            UIEventListener.Get(btnReturn).onClick = delegate
            {
                UICenterMasterManager.Instance.ReturnWindow();
            };

            UIEventListener.Get(btnShop).onClick = delegate
            {
                UICenterMasterManager.Instance.ShowWindow(WindowID.WindowID_Shop);
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
    }
}
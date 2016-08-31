using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    public class UIShopWindow : UIBaseWindow
    {
        protected override void SetWindowId()
        {
            this.ID = WindowID.WindowID_Shop;
        }

        public override void InitWindowOnAwake()
        {
            base.InitWindowOnAwake();
            InitWindowCoreData();
        }

        protected override void InitWindowCoreData()
        {
            base.InitWindowCoreData();
            this.preWindowID = WindowID.WindowID_MainMenu;

            this.windowData.windowType = UIWindowType.PopUp;
            this.windowData.navigationMode = UIWindowNavigationMode.NormalNavigation;
            this.windowData.colliderMode = UIWindowColliderMode.WithBg;
        }

        public override void OnAddColliderBg(GameObject obj)
        {
            UIEventListener.Get(obj).onClick = delegate
            {
                UICenterMasterManager.Instance.ReturnWindow();
                Debuger.Log("## On click Shop window's background collider ##");
            };
        }
    }
}



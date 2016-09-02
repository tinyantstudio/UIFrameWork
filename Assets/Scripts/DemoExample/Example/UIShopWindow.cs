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
            this.windowData.windowType = UIWindowType.PopUp;
            this.windowData.showMode = UIWindowShowMode.DoNothing;
            this.windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            this.windowData.colliderMode = UIWindowColliderMode.WithBg;
        }

        public override void OnAddColliderBg(GameObject obj)
        {
            UIEventListener.Get(obj).onClick = delegate
            {
                // UICenterMasterManager.Instance.PopNavigationWindow();
                UICenterMasterManager.Instance.CloseWindow(this.ID);
                Debuger.Log("<color=green>## [UIShop] ##</color> On click Shop window's background collider ##");
            };
        }
    }
}



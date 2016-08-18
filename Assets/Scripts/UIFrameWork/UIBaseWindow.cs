using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    /// <summary>
    /// 窗口基类
    /// </summary>
    public class UIBaseWindow : MonoBehaviour
    {
        protected UIPanel originPanel;

        // 如果需要可以添加一个BoxCollider屏蔽事件
        private bool isLock = false;
        protected bool isShown = false;

        // Current windowID
        protected WindowID windowID = WindowID.WindowID_Invaild;

        // if there is no BackSequece Data just check the preWindowID
        // Try open preWindowID
        protected WindowID preWindowID = WindowID.WindowID_Invaild;
        public WindowData windowData = new WindowData();

        // Return Logic when leaving current window
        private event BoolDelegate returnPreLogic = null;

        protected Transform mTrs;
        protected virtual void Awake()
        {
            this.gameObject.SetActive(true);
            mTrs = this.gameObject.transform;
            InitWindowOnAwake();
        }

        public bool IsLock
        {
            get { return isLock; }
            set { isLock = value; }
        }

        private int minDepth = 1;
        public int MinDepth
        {
            get { return minDepth; }
            set { minDepth = value; }
        }

        public WindowID GetID
        {
            get
            {
                if (this.windowID == WindowID.WindowID_Invaild)
                    Debug.LogError("window id is " + WindowID.WindowID_Invaild);
                return windowID;
            }
            private set { windowID = value; }
        }

        public WindowID GetPreWindowID
        {
            get { return preWindowID; }
            private set { preWindowID = value; }
        }

        // Need to added to back seq data
        public bool CanAddedToBackSeq
        {
            get
            {
                if (this.windowData.windowType == UIWindowType.PopUp)
                    return false;
                if (this.windowData.windowType == UIWindowType.Fixed)
                    return false;
                if (this.windowData.showMode == UIWindowShowMode.NoNeedBack)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// 界面是否要刷新BackSequence数据
        /// 1.显示NoNeedBack或者从NoNeedBack显示新界面 不更新BackSequenceData(隐藏自身即可)
        /// 2.HideOther
        /// 3.NeedBack
        /// </summary>
        public bool RefreshBackSeqData
        {
            get
            {
                if (this.windowData.showMode == UIWindowShowMode.HideOther
                    || this.windowData.showMode == UIWindowShowMode.NeedBack)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Called on Awake() used for window data Init
        /// </summary>
        public virtual void InitWindowOnAwake()
        {

        }

        /// <summary>
        /// Get the current window's manager
        /// </summary>
        public UIManagerBase GetWindowManager
        {
            get
            {
                UIManagerBase baseManager = this.gameObject.GetComponent<UIManagerBase>();
                return baseManager;
            }
            private set { }
        }

        /// <summary>
        /// Reset the window
        /// </summary>
        public virtual void ResetWindow()
        {
        }

        /// <summary>
        /// Init the window core data
        /// </summary>
        protected virtual void InitWindowData()
        {
        }

        public virtual void ShowWindow()
        {
            isShown = true;
            NGUITools.SetActive(this.gameObject, true);
        }

        public virtual void HideWindow(Action action = null)
        {
            IsLock = true;
            isShown = false;
            NGUITools.SetActive(this.gameObject, false);
            if (action != null)
                action();
        }

        public void HideWindowDirectly()
        {
            IsLock = true;
            isShown = false;
            NGUITools.SetActive(this.gameObject, false);
        }

        public virtual void DestroyWindow()
        {
            BeforeDestroyWindow();
            GameObject.Destroy(this.gameObject);
        }

        protected virtual void BeforeDestroyWindow()
        {
        }

        /// <summary>
        /// Register call back method before the window returned(closed)
        /// Case: when you exit a window to pop up a confirm MessageBox
        /// </summary>
        protected void RegisterReturnLogic(BoolDelegate newLogic)
        {
            returnPreLogic = newLogic;
        }

        public bool ExecuteReturnLogic()
        {
            if (returnPreLogic == null)
                return false;
            else
                return returnPreLogic();
        }
    }
}

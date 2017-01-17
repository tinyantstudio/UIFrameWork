using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    /// <summary>
    /// Base window
    /// </summary>
    public abstract class UIWindowBase : MonoBehaviour
    {
        protected UIPanel originPanel;

        // Lock state
        // When your window is in loading state
        // You can check IsLock to enable some button click method and so on
        // or you Add a very high depth collider instead
        private bool isLock = false;
        // in showing
        protected bool isShown = false;
        // Current windowID
        private WindowID windowID = WindowID.WindowID_Invaild;

        // if there is no BackSequece Data just check the preWindowID
        // Try open preWindowID
        protected WindowID preWindowID = WindowID.WindowID_Invaild;
        // Core window data must be init before open the Window
        public WindowCoreData windowData = new WindowCoreData();

        // Return Logic when leaving current window
        private event BoolDelegate returnPreLogic = null;

        protected Transform mTrs;
        protected virtual void Awake ()
        {
            this.gameObject.SetActive(true);
            mTrs = this.gameObject.transform;
            SetWindowId();
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

        public WindowID ID
        {
            get
            {
                if (this.windowID == WindowID.WindowID_Invaild)
                    Debug.LogError("window id is " + WindowID.WindowID_Invaild);
                return windowID;
            }
            protected set { windowID = value; }
        }

        public WindowID PreWindowID
        {
            get { return preWindowID; }
            private set { preWindowID = value; }
        }

        // Need to added to back seq data
        public bool CanAddedToBackSeq
        {
            get { return this.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation; }
        }

        // Need Refresh the back seq data
        public bool RefreshBackSeqData
        {
            get { return this.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation; }
        }

        // Set the window Id use 
        protected abstract void SetWindowId ();

        /// <summary>
        /// Called on Awake() used for window data Init
        /// </summary>
        public virtual void InitWindowOnAwake ()
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
        public virtual void ResetWindow ()
        {
        }

        /// <summary>
        /// Init the window core data
        /// </summary>
        protected virtual void InitWindowCoreData ()
        {
        }

        public virtual void ShowWindow ( BaseWindowContextData contextData = null )
        {
            isShown = true;
            isLock = false;
            NGUITools.SetActive(this.gameObject, true);
        }

        public virtual void HideWindow ( Action action = null )
        {
            IsLock = false;
            isShown = false;
            NGUITools.SetActive(this.gameObject, false);
            if (action != null)
                action();
        }

        public void HideWindowDirectly ()
        {
            IsLock = false;
            isShown = false;
            NGUITools.SetActive(this.gameObject, false);
        }

        public virtual void DestroyWindow ()
        {
            BeforeDestroyWindow();
            GameObject.Destroy(this.gameObject);
        }

        protected virtual void BeforeDestroyWindow ()
        {
        }

        // On Add Collider bg to window
        // Add collider bg click event
        public virtual void OnAddColliderBg ( GameObject obj )
        {

        }

        /// <summary>
        /// Register call back method before the window returned(closed)
        /// Case: when you exit a window to pop up a confirm MessageBox
        /// </summary>
        protected void RegisterReturnLogic ( BoolDelegate newLogic )
        {
            returnPreLogic = newLogic;
        }

        public bool ExecuteReturnLogic ()
        {
            if (returnPreLogic == null)
                return false;
            else
                return returnPreLogic();
        }
    }
}

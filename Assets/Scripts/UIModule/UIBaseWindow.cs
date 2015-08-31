using UnityEngine;
using System.Collections;
using System;

namespace CoolGame
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

        // 当前界面ID
        protected WindowID windowID = WindowID.WindowID_Invaild;

        // 指向上一级界面ID(BackSequence无内容，返回上一级)
        protected WindowID preWindowID = WindowID.WindowID_Invaild;
        public WindowData windowData = new WindowData();

        // Return处理逻辑
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

        /// <summary>
        /// 能否添加到导航数据中
        /// </summary>
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
        /// 在Awake中调用，初始化界面(给界面元素赋值操作)
        /// </summary>
        public virtual void InitWindowOnAwake()
        {
        }

        /// <summary>
        /// 获得该窗口管理类
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
        /// 重置窗口
        /// </summary>
        public virtual void ResetWindow()
        {
        }

        /// <summary>
        /// 初始化窗口数据
        /// </summary>
        public virtual void InitWindowData()
        {
            if (windowData == null)
                windowData = new WindowData();
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
        /// 界面在退出或者用户点击返回之前都可以注册执行逻辑
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

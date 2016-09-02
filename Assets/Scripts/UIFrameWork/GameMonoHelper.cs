using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    public class GameMonoHelper : MonoBehaviour
    {
        private static GameMonoHelper instance;

        private AsyncOperation op;
        public static GameMonoHelper GetInstance()
        {
            return instance;
        }

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void LoadGameScene(string strSceneName, bool closeAllUI, Action callBack)
        {
            StartCoroutine(_LoadGameScene(strSceneName, closeAllUI, callBack));
        }

        IEnumerator _LoadGameScene(string strSceneName, bool closeAllUI, Action callBack)
        {
            op = Application.LoadLevelAsync(strSceneName);
            while (!op.isDone)
                yield return 0;

            if (closeAllUI)
                UICenterMasterManager.Instance.HideAllShownWindow(true);

            if (callBack != null)
                callBack();
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
        }
    }
}
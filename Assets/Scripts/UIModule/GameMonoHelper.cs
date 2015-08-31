using UnityEngine;
using System.Collections;
using System;

namespace CoolGame
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

        public void LoadGameScene(string strSceneName, Action callBack)
        {
            StartCoroutine(_LoadGameScene(strSceneName, callBack));
        }

        IEnumerator _LoadGameScene(string strSceneName, Action callBack)
        {
            op = Application.LoadLevelAsync(strSceneName);
            while (!op.isDone)
                yield return 0;

            if (callBack != null)
                callBack();

            // http://answers.unity3d.com/questions/348974/edit-camera-culling-mask.html
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
        }
    } 
}


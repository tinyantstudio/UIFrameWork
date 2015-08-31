using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoolGame
{
    public class TestUIModule : MonoBehaviour
    {
        public GameObject targetObj;

        public List<GameObject> listToDestroy;
        public void ShowRankWindow()
        {
            UIManager.GetInstance().ShowWindow(WindowID.WindowID_Rank);
            UIManager.GetInstance().ShowWindow(WindowID.WindowID_MainMenu);
        }

        public void HideRankWindow()
        {
            UIManager.GetInstance().HideWindow(WindowID.WindowID_Rank);
            UIManager.GetInstance().ShowWindow(WindowID.WindowID_TopBar);
        }


        private int flag = 1;
        public void HideObj()
        {
            /*if (targetObj.activeSelf)
                targetObj.SetActive(false);
            else
                targetObj.SetActive(true);*/

            // targetObj.GetComponent<UISprite>().alpha = flag * 1.0f;
            flag = (flag == 1 ? 0 : 1);
            targetObj.transform.localPosition += new UnityEngine.Vector3(100.0f, 0.0f, 0.0f);
        }
    }
}


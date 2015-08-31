using UnityEngine;
using System.Collections;

namespace CoolGame
{
    public class UILevelItem : MonoBehaviour
    {
        private UILabel lbLevelName;
        private GameObject btnLevelItem;

        void Awake()
        {
            lbLevelName = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Label");
            btnLevelItem = GameUtility.FindDeepChild(this.gameObject, "Icon").gameObject;

            UIEventListener.Get(btnLevelItem).onClick = delegate
            {
                UIManager.GetInstance().ShowWindow(WindowID.WindowID_LevelDetail);
                // UIManager.GetInstance().ShowWindowDelay(2.0f, WindowID.WindowID_LevelDetail);
            };
        }

        public void SetData(string levelName)
        {
            lbLevelName.text = levelName;
        }
    }
}


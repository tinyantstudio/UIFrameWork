using UnityEngine;
using System.Collections;

namespace TinyFrameWork
{
    public class UILevelItem : MonoBehaviour
    {
        private UILabel lbLevelName;
        private GameObject btnLevelItem;

        private int starCount = 0;
        void Awake()
        {
            lbLevelName = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Label");
            btnLevelItem = GameUtility.FindDeepChild(this.gameObject, "Icon").gameObject;

            UIEventListener.Get(btnLevelItem).onClick = delegate
            {
                // UICenterMasterManager.GetInstance().ShowWindow(WindowID.WindowID_LevelDetail);
                ShowWindowData showData = new ShowWindowData();
                ContextDataLevelDetail context = new ContextDataLevelDetail();
                context.levelDescription = "Hi Man cool things will be happen.Be busy living or be busy dying. - Fighting for the game world";
                context.levelId = 10001;
                context.levelName = lbLevelName.text;
                context.starCount = starCount;
                showData.contextData = context;

                UICenterMasterManager.GetInstance().ShowWindow(WindowID.WindowID_LevelDetail, showData);
                // UICenterMasterManager.GetInstance().ShowWindowDelay(1.0f, WindowID.WindowID_LevelDetail, showData);
            };
        }

        public void SetData(string levelName, int count)
        {
            lbLevelName.text = levelName;
            starCount = count;
        }
    }
}


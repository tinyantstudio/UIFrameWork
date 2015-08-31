using UnityEngine;
using System.Collections;

namespace CoolGame
{
    public class UIRankItem : MonoBehaviour
    {

        private UILabel lbItemName;
        private UISprite spIcon;
        private GameObject btnBg;

        /// <summary>
        /// Item需要编写通用类
        /// </summary>
        void Awake()
        {
            lbItemName = GameUtility.FindDeepChild<UILabel>(this.gameObject, "Label");
            spIcon = GameUtility.FindDeepChild<UISprite>(this.gameObject, "Sprite");
            btnBg = GameUtility.FindDeepChild(this.gameObject, "Bgbtn").gameObject;

            UIEventListener.Get(btnBg).onClick = OnBtnClick;
        }

        public void InitItem(string playerName, string iconName)
        {
            lbItemName.text = playerName;
            spIcon.spriteName = iconName;
        }


        public void OnBtnClick(GameObject obj)
        {
            // item 被点击

            UIRankManager.GetInstance().ShowWindow(WindowID.WindowID_Rank_Detail);
            UIRankDetail detail = (UIRankDetail)UIRankManager.GetInstance().GetGameWindow(WindowID.WindowID_Rank_Detail);
            detail.UpdateDetailData(lbItemName.text, spIcon.spriteName);

            Debug.Log("Item clicked.");
        }
    }

}

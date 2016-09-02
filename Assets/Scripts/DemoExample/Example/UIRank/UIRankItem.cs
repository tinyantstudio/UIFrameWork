using UnityEngine;
using System.Collections;

namespace TinyFrameWork
{
    /// <summary>
    /// Demo rank item to show the rank detail content
    /// </summary>
    public class UIRankItem : MonoBehaviour
    {
        private UILabel lbItemName;
        private UISprite spIcon;
        private GameObject btnBg;

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
            // To show Rank logic's child window(Rank_Detail)
            UIRankManager.GetInstance().ShowWindow(WindowID.WindowID_Rank_Detail);
            UIRankDetail detail = (UIRankDetail)UIRankManager.GetInstance().GetGameWindow(WindowID.WindowID_Rank_Detail);
            detail.UpdateDetailData(lbItemName.text, spIcon.spriteName);
            Debuger.Log("<color=green>[##UIRank##]</color> UIRank Item clicked to show the Rank_Detail window.");
        }
    }
}

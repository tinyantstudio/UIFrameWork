using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    // Demo WindowId for UIFrameWork's test
    public enum WindowID
    {
        WindowID_Invaild = 0,
        WindowID_Rank,          // 排行榜界面
        WindowID_Rank_Detail,   // 排行榜详情界面
        WindowID_Rank_OwnDetail,
        WindowID_Level,
        WindowID_LevelDetail,
        WindowID_Matching,
        WindowID_MatchResult,
        WindowID_Skill,
        WindowID_MainMenu,
        WindowID_TopBar,
        WindowID_MessageBox,
    }

    public enum UIWindowType
    {
        Normal,    // 可推出界面(UIMainMenu,UIRank等)
        Fixed,     // 固定窗口(UITopBar等)
        PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
    }

    /// <summary>
    /// 1. HideOther (close or hide other window when open HideOther window)
    /// 2. NeedBack ()
    /// 3. NoNeedBack
    /// </summary>
    public enum UIWindowShowMode
    {
        DoNothing,
        HideOther,     // 闭其他界面()
        NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
        NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
    }

    public enum UIWindowColliderMode
    {
        None,      // No BgTexture and No Collider
        Normal,    // Collider with alpha 0.001 BgTexture
        WithBg,    // Collider with alpha 1 BgTexture
    }

    public class UIResourceDefine
    {
        public static Dictionary<int, string> windowPrefabPath = new Dictionary<int, string>()
        {
            { (int)WindowID.WindowID_Rank, "UIRank" },
            { (int)WindowID.WindowID_Level, "UILevelWindow" },
            { (int)WindowID.WindowID_MainMenu, "UIMainMenu" },
            { (int)WindowID.WindowID_TopBar, "UITopBar" },
            { (int)WindowID.WindowID_MessageBox, "UIMessageBox" },
            { (int)WindowID.WindowID_LevelDetail, "UILevelDetailWindow" },
            { (int)WindowID.WindowID_Matching, "UIMatching" },
            { (int)WindowID.WindowID_MatchResult, "UIMatchResult" },
            { (int)WindowID.WindowID_Skill, "UISkill" }
        };

        public static string UIPrefabPath = "UIPrefab/";
    }
}

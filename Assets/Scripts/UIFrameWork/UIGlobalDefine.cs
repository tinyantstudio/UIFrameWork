using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    // Demo WindowId for UIFrameWork's test
    public enum WindowID
    {
        WindowID_Invaild = 0,
        WindowID_Rank,                  // 排行榜界面
        WindowID_Rank_Detail,           // 排行榜详情界面
        WindowID_Rank_OwnDetail,        // 排行榜个人详情
        WindowID_Level,                 // 推图界面
        WindowID_LevelDetail,           // 关卡详情
        WindowID_Matching,              // 比赛界面
        WindowID_MatchResult,           // 比赛结果界面
        WindowID_Skill,                 // 技能界面
        WindowID_MainMenu,              // 主界面
        WindowID_TopBar,                // 顶部界面
        WindowID_MessageBox,            // 通用弹框
    }

    public enum UIWindowType
    {
        Normal,    // 可推出界面(UIMainMenu,UIRank等)
        Fixed,     // 固定窗口(UITopBar等)
        PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
    }

    /// <summary>
    /// 1. HideOther (close or hide other window when open HideOther window add to navigation sequence data)
    /// 2. NeedBack (open window don't close other window add to navigation sequence data)
    /// 3. NoNeedBack (open window close other window no need add to navigation sequence data)
    /// 
    /// 8.22 Divide UIWindowShowMode to UIWindowShowMode and new UIWindowNavigationMode
    /// ShowMode control the window show mode
    /// NavigationMode control the navigation system
    /// </summary>
    public enum UIWindowShowMode
    {
        //DoNothing,
        //HideOther,     // 打开界面关闭其他界面
        //NeedBack,      // 打开界面不关闭其他界面
        //NoNeedBack,    // 打开界面关闭其他界面，不加入导航队列

        DoNothing = 0,
        HideOtherWindow,
        DestoryOtherWindow,
    }

    public enum UIWindowNavigationMode
    {
        IgnoreNavigation = 0,
        NeedAdded,
    }

    public enum UIWindowColliderMode
    {
        None,      // No BgTexture and No Collider
        Normal,    // Collider with alpha 0.001 BgTexture
        WithBg,    // Collider with alpha 1 BgTexture
    }

    public class UIResourceDefine
    {
        // Define the UIWindow prefab paths
        // all window prefab placed in Resources folder
        // maybe your window assetbundle path
        public static Dictionary<int, string> windowPrefabPath = new Dictionary<int, string>()
        {
            { (int)WindowID.WindowID_Rank,        "UIRank" },
            { (int)WindowID.WindowID_Level,       "UILevelWindow" },
            { (int)WindowID.WindowID_MainMenu,    "UIMainMenu" },
            { (int)WindowID.WindowID_TopBar,      "UITopBar" },
            { (int)WindowID.WindowID_MessageBox,  "UIMessageBox" },
            { (int)WindowID.WindowID_LevelDetail, "UILevelDetailWindow" },
            { (int)WindowID.WindowID_Matching,    "UIMatching" },
            { (int)WindowID.WindowID_MatchResult, "UIMatchResult" },
            { (int)WindowID.WindowID_Skill,       "UISkill" }
        };

        // Main folder
        public static string UIPrefabPath = "UIPrefab/";
    }
}
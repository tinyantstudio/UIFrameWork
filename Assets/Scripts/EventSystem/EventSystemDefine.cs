using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    public enum HandleType
    {
        Add = 0,
        Remove = 1,
    }

    public class EventSystemDefine
    {
        public static Dictionary<int, string> dicHandleType = new Dictionary<int, string>()
        {
            { (int)HandleType.Add, "Add"},
            { (int)HandleType.Remove, "Remove"},
        };
    }

    public enum EventId
    {
        None = 0,
        // Test User Input
        TestUserInput,
        // UIFrameWork Event id
        PopRootWindowAdded,
        // Common Event id
        CoinChange,
        DiamondChange,
        // Player Common Event id
        PlayerHitByAI,
        // Net message
        NetUpdateMailContent,
    }
}

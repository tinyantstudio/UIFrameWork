using UnityEngine;
using System.Collections;

namespace TinyFrameWork
{
    public class EventSystemDefine
    {
        // test message.
        public const string EventTestUserInput = "event_test_user_input";

        // player common message.
        public const string EventCommonCoinChange = "event_coin_change";
        public const string EventCommonDiamondChange = "event_diamond_change";

        // update UI when receive net message.
        public const string EventNetUpdateMailContent = "event_update_mail_content";

        // or other battle message.
        public const string EventPlayerHitByAI = "event_match_hitby_ai";

        // UIFrameWork Message
        public const string EventUIFrameWorkPopRootWindowAdded = "ufprw";
    }
}

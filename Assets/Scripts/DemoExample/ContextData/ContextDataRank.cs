using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContextDataRank : BaseWindowContextData
{
    public List<RankItemData> listRankItemDatas { get; set; }
}

public class RankItemData
{
    public string playerName { get; set; }
    public string headIcon { get; set; }
    public string playerDescription { get; set; }
}
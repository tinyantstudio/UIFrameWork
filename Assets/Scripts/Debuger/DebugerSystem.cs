using UnityEngine;
using System.Collections;

public class DebugerSystem : MonoBehaviour{

    public bool EnableSystem = false;

    [System.Serializable]
    public class DBSystemStr
    {
        //debug string define
        public string DBStrBattleSystem = "BattleSystem# ";
        public string DBStrUISystem = "UISystem# ";
    }

    public DBSystemStr DebugString;

    static private DebugerSystem instance;
    static public DebugerSystem Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debuger.EnableLog = EnableSystem;
    }
    
}

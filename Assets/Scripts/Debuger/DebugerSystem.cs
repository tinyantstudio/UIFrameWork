using UnityEngine;
using System.Collections;

public class DebugerSystem : MonoBehaviour
{
    // Switch to enable the debug system
    public bool EnableSystem = false;

    static private DebugerSystem instance;
    static public DebugerSystem Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        Debuger.EnableLog = EnableSystem;
    }
}

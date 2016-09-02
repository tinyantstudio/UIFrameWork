using UnityEngine;
using System.Collections;

public class ShowWindowData
{
    // Reset window
    public bool forceResetWindow = false;
    // force clear the navigation data
    public bool forceClearBackSeqData = false;
    // Object (pass data to target showed window)
    public BaseWindowContextData contextData;
    // Execute the navigation logic
    public bool executeNavLogic = true;
    // Check navigation 
    public bool checkNavigation = false;
    // force ignore add nav data
    public bool ignoreAddNavData = false;
}

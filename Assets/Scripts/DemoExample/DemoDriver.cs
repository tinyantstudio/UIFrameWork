using UnityEngine;
using System.Collections;

public class DemoDriver : MonoBehaviour
{
    IEnumerator Start()
    {
        // Ensure the UIFrameWork init properly
        // You can check the Script Execute Order make UICenterMasterManager.cs to ahead other script.
        yield return new WaitForEndOfFrame();

        // Demo driver to show first two core window
        // MainMenu and TopBar so you can enter the really cool demo
        // Just click some game logic button to feel the frameWork's doing

        // You can switch the Debuger state ON/OFF

        TinyFrameWork.UICenterMasterManager.Instance.ShowWindow(TinyFrameWork.WindowID.WindowID_MainMenu);
        TinyFrameWork.UICenterMasterManager.Instance.ShowWindow(TinyFrameWork.WindowID.WindowID_TopBar);
    }
}

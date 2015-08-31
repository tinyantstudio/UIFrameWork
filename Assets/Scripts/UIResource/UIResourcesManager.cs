using UnityEngine;
using System.Collections;

public class UIResourcesManager : MonoBehaviour
{
    public GameObject[] destroyObj;
    public GameObject objDontDestroy;

    public GameObject objRoot;

    public void LoadEmptyScene()
    {
        DontDestroyOnLoad(objRoot);
        DontDestroyOnLoad(objDontDestroy);

        for (int i = 0; i < destroyObj.Length; i++)
        {
            if (destroyObj[i] != null)
                GameObject.Destroy(destroyObj[i]);
        }

        Application.LoadLevel("TestEmptyScene");
    }
}

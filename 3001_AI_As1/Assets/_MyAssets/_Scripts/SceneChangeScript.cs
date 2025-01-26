using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeScript : MonoBehaviour
{
    public void ChangeSceneTo(int sceneIndexToLoad)
    {
        SceneLoader.LoadSceneByIndex(sceneIndexToLoad);
    }
}


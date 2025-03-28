using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Start Scene")
        {
            SceneManager.LoadScene("Play Scene");
        }
    }
}

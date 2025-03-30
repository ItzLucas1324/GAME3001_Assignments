using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SceneChange()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Start Scene")
        {
            SceneManager.LoadScene("Play Scene");
        }
    }

    public void ReturnToStart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        if (currentSceneName == "Win Scene" || currentSceneName == "Loser Scene")
        {
            SceneManager.LoadScene("Start Scene");
            GameManager.Instance.SoundManager.PlayMusic("Pink Panther");
        }
    }

    public void YouLose()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Play Scene")
        {
            SceneManager.LoadScene("Loser Scene");
            GameManager.Instance.SoundManager.PlayMusic("Losing Tune");
        }
    }

    public void YouWin()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Play Scene")
        {
            SceneManager.LoadScene("Win Scene");
            GameManager.Instance.SoundManager.PlayMusic("Victory Tune");
        }
    }
}

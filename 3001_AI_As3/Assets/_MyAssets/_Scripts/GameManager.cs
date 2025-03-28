using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SoundManager SoundManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SoundManager = GetComponentInChildren<SoundManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Music
        SoundManager.AddSound("Pink Panther", Resources.Load<AudioClip>("Audio/01 - The Pink Panther Theme"), SoundType.SOUND_MUSIC);
        GameManager.Instance.SoundManager.PlayMusic("Pink Panther");
    }
}

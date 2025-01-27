using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip introMusic;
    [SerializeField] AudioClip mainMusic;
    
    private AudioSource introAudioSource;
    private AudioSource mainAudioSource;
    private static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameObject introAudioObj = new GameObject("IntroAudio");
        GameObject mainAudioObj = new GameObject("MainAudio");
        
        introAudioObj.transform.parent = transform;
        mainAudioObj.transform.parent = transform;
        
        introAudioSource = introAudioObj.AddComponent<AudioSource>();
        mainAudioSource = mainAudioObj.AddComponent<AudioSource>();
        
        introAudioSource.volume = 0.5f;
        mainAudioSource.volume = 0.5f;
        introAudioSource.loop = false;
        mainAudioSource.loop = true;
        
        PlayMusic();
    }

    private void PlayMusic()
    {
        introAudioSource.clip = introMusic;
        mainAudioSource.clip = mainMusic;
        
        introAudioSource.Play();
        
        StartCoroutine(MonitorIntroMusic());
    }

    private System.Collections.IEnumerator MonitorIntroMusic()
    {
        
        while (introAudioSource.time < introMusic.length - 0.1f)
        {
            yield return null;
        }
        
        mainAudioSource.Play();
        
        float fadeTime = 0.1f;
        float startVolume = introAudioSource.volume;
        
        while (introAudioSource.volume > 0)
        {
            introAudioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        introAudioSource.Stop();
    }
}
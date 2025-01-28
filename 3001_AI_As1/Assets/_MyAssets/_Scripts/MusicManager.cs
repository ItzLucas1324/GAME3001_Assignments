using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip mainMusic;
    private AudioSource audioSource;
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
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.loop = false;
    }

    private void Start()
    {
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {       
            audioSource.clip = introMusic;
            audioSource.Play();

            yield return new WaitForSeconds(introMusic.length);
                
            audioSource.clip = mainMusic;
            audioSource.loop = true;
            audioSource.Play();
        
    }
}

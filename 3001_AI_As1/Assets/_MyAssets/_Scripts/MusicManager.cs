using System.Collections;
using System.Collections.Generic;
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
        introAudioSource = gameObject.AddComponent<AudioSource>();
        mainAudioSource = gameObject.AddComponent<AudioSource>();

        introAudioSource.volume = 0.5f;
        mainAudioSource.volume = 0.5f;
        introAudioSource.loop = false;
        mainAudioSource.loop = true;

        PlayMusic();
    }
    private void PlayMusic()
    {
        introAudioSource.clip = introMusic;
        introAudioSource.Play();

        double introEndTime = AudioSettings.dspTime + introMusic.length;
        mainAudioSource.clip = mainMusic;
        mainAudioSource.PlayScheduled(introEndTime);
    }
}

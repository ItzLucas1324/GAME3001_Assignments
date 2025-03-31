using UnityEngine;
using System.Collections.Generic;

public enum SoundType
{
    SOUND_SFX,
    SOUND_MUSIC
}

public class SoundManager : MonoBehaviour
{
    private float sfxVolume = 1f;
    private float musicVolume = 1f;
    private float masterVolume = 1f;

    private AudioSource sfxSource;
    private AudioSource loopingSFXSource;
    public AudioSource musicSource;

    private Dictionary<string, AudioClip> sfxDictionary;
    private Dictionary<string, AudioClip> musicDictionary;

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        loopingSFXSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        sfxDictionary = new Dictionary<string, AudioClip>();
        musicDictionary = new Dictionary<string, AudioClip>();

        loopingSFXSource.loop = true;
        musicSource.loop = true;
    }

    public void AddSound(string soundKey, AudioClip audioClip, SoundType soundType)
    {
        if (audioClip == null)
        {
            Debug.LogError("Error: AudioClip is null for key: " + soundKey);
            return;
        }

        switch (soundType)
        {
            case SoundType.SOUND_SFX:
                if (!sfxDictionary.ContainsKey(soundKey))
                {
                    sfxDictionary.Add(soundKey, audioClip);
                }
                else
                {
                    Debug.LogWarning("Key: " + soundKey + " already found in SFX Dictionary.");
                }
                break;
            case SoundType.SOUND_MUSIC:
                if (!musicDictionary.ContainsKey(soundKey))
                {
                    musicDictionary.Add(soundKey, audioClip);
                }
                else
                {
                    Debug.LogWarning("Key: " + soundKey + " already found in Music Dictionary.");
                }
                break;
            default:
                Debug.LogError("Unsupported sound type for key: " + soundKey);
                break;
        }
    }

    public void PlaySound(string soundKey, float volume = 1f)
    {
        if (sfxDictionary.ContainsKey(soundKey))
        {
            AudioClip clip = sfxDictionary[soundKey];
            sfxSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogError("Sound key " + soundKey + " not found in SFX Dictionary.");
        }
    }

    public void PlayLoopingSound(string soundKey)
    {

        if (sfxDictionary.ContainsKey(soundKey))
        {
            if (loopingSFXSource.isPlaying && loopingSFXSource.clip == sfxDictionary[soundKey])
                return;

            loopingSFXSource.Stop();
            loopingSFXSource.clip = sfxDictionary[soundKey];
            loopingSFXSource.Play();
        }
        else
        {
            Debug.LogError("Sound key " + soundKey + " not found in SFX Dictionary.");
        }
    }

    public void StopLoopingSound()
    {
        if (loopingSFXSource.isPlaying && loopingSFXSource.loop)
        {
            loopingSFXSource.Stop();
            loopingSFXSource.clip = null;
        }
    }


    public void PlayMusic(string soundKey, bool loop = true)
    {
        if (musicDictionary.ContainsKey(soundKey))
        {
            musicSource.Stop();
            musicSource.clip = musicDictionary[soundKey];
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogError("Sound key " + soundKey + " not found in Music Dictionary.");
        }
    }

    public void SetVolumeSFX(float volume)
    {
        sfxVolume = volume * masterVolume;
        sfxSource.volume = sfxVolume;
        loopingSFXSource.volume = sfxVolume;
    }

    public void SetVolumeMusic(float volume)
    {
        musicVolume = volume * masterVolume;
        musicSource.volume = musicVolume;
    }

    public void SetVolumeMaster(float volume)
    {
        masterVolume = volume;
        sfxSource.volume = sfxVolume * masterVolume;
        loopingSFXSource.volume = sfxVolume * masterVolume;
        musicSource.volume = musicVolume * masterVolume;
    }

    public float GetClipLength(string soundKey)
    {
        if (musicDictionary.ContainsKey(soundKey))
        {
            return musicDictionary[soundKey].length;
        }
        else
        {
            Debug.LogError("Sound key " + soundKey + " not found in Music Dictionary.");
            return 0f;
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();

    }public void StopSound()
    {
        sfxSource.Stop();
    }
}

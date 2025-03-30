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
        SoundManager.SetVolumeMusic(0.45f);
        SoundManager.SetVolumeSFX(1.0f);

        // Music
        SoundManager.AddSound("Pink Panther", Resources.Load<AudioClip>("Audio/01 - The Pink Panther Theme"), SoundType.SOUND_MUSIC);
        SoundManager.AddSound("Victory Tune", Resources.Load<AudioClip>("Audio/27. Victory Tune"), SoundType.SOUND_MUSIC);
        SoundManager.AddSound("Losing Tune", Resources.Load<AudioClip>("Audio/Snakeskin Boots Instrumental"), SoundType.SOUND_MUSIC);
        GameManager.Instance.SoundManager.PlayMusic("Pink Panther");

        // SFX
        SoundManager.AddSound("Player Walking", Resources.Load<AudioClip>("Audio/squidward-walking"), SoundType.SOUND_SFX);
        SoundManager.AddSound("Wall Bump", Resources.Load<AudioClip>("Audio/bumpintowall_pokemon"), SoundType.SOUND_SFX);
    }
}

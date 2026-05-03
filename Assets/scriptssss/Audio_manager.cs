using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Audio_manager : MonoBehaviour
{
    public static Audio_manager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Lists")]
    public List<AudioClip> bgmClips;
    public List<AudioClip> sfxClips;

    [Header("Special SFX")]
    public List<AudioClip> breakClips;
    public List<AudioClip> swordClips;
    float pitchmin = 0.1f;
    float pitchmax = 0.9f;


    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();

    [Header("Settings")]
    [Range(0f, 1f)] public float bgmVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    void InitDictionaries()
    {
        // Convert lists to dictionaries using clip name
        foreach (var clip in sfxClips)
        {
            if (!sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }

        foreach (var clip in bgmClips)
        {
            if (!bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }
    }

    // ------------------- //
    // 🎵 BGM
    // ------------------- //

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlayBGM(string name)
    {
        if (bgmDict.TryGetValue(name, out AudioClip clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning("BGM not found: " + name);
        }
    }

    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Count)
        {
            PlayBGM(bgmClips[index]);
        }
    }

    // ------------------- //
    // 🔊 SFX
    // ------------------- //

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Count)
        {
            PlaySFX(sfxClips[index]);
        }
    }

    public void PlaySFX(string name, float pitchMin, float pitchMax)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.pitch = Random.Range(pitchMin, pitchMax);
            sfxSource.PlayOneShot(clip, sfxVolume);
            sfxSource.pitch = 1f;
        }
    }

    // ------------------- //
    // UTIL
    // ------------------- //


    public void PlayBreak()
    {
        if (breakClips == null || breakClips.Count == 0) return;

        int index = Random.Range(0, breakClips.Count);

        sfxSource.pitch = Random.Range(pitchmin, pitchmax);
        sfxSource.PlayOneShot(breakClips[index], sfxVolume);
        sfxSource.pitch = 1f;
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
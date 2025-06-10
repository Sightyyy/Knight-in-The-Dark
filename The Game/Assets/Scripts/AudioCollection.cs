using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NamedAudioClip
{
    public string name;
    public AudioClip clip;
}
public class AudioCollection : MonoBehaviour
{
    [Header("========== Output ==========")]
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource SFX;
    [SerializeField] private AudioSource VO;

    [Header("========== Background Music ==========")]
    public AudioClip mainMenu;
    public AudioClip game;
    public AudioClip gameOver;

    [Header("========== SFX ==========")]
    public AudioClip buttonClick;
    public AudioClip death;
    public AudioClip death2;
    public AudioClip walking;
    public AudioClip sensor;
    public AudioClip jump;
    public AudioClip checkpoint;
    [Header("========== Voice Over (VO) ==========")]
    public AudioClip vo1;
    public AudioClip vo2;
    public AudioClip vo3;
    public AudioClip vo4;
    public AudioClip vo5;
    public AudioClip vo6;
    public AudioClip vo7;
    public AudioClip vo8;
    public AudioClip vo9;
    public AudioClip vo10;
    public NamedAudioClip[] voiceOvers;

    private List<AudioSource> sfxSources = new List<AudioSource>();

    public void PlayBGM(AudioClip clip)
    {
        BGM.clip = clip;
        BGM.loop = true;
        BGM.Play();
    }
    public void PlaySound(AudioClip clip)
    {
        SFX.clip = clip;
        SFX.PlayOneShot(clip);
    }

    public void StopPlayBGM()
    {
        BGM.Stop();
    }

    public void PauseBGM()
    {
        if (BGM.isPlaying)
        {
            BGM.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (!BGM.isPlaying)
        {
            BGM.UnPause();
        }
    }

    public AudioSource PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource availableSource = GetAvailableAudioSource();
        availableSource.clip = clip;
        availableSource.volume = volume;
        availableSource.Play();
        return availableSource;
    }

    public void StopSFX()
    {
        foreach (var source in sfxSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void PlayVO(AudioClip clip)
    {
        if (clip != null && VO != null)
        {
            VO.clip = clip;
            VO.Play();
        }
    }
    public void StopVO()
    {
        foreach (var source in sfxSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public AudioClip GetClipByName(string name)
    {
        foreach (var item in voiceOvers)
        {
            if (item.name == name)
                return item.clip;
        }
        return null;
    }
    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        sfxSources.Add(newSource);
        return newSource;
    }
}
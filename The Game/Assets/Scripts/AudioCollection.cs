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
    private List<AudioSource> voSource = new List<AudioSource>();

    private void Awake()
    {
        voiceOvers = new NamedAudioClip[10];
        voiceOvers[0] = new NamedAudioClip { name = "VO1", clip = vo1 };
        voiceOvers[1] = new NamedAudioClip { name = "VO2", clip = vo2 };
        voiceOvers[2] = new NamedAudioClip { name = "VO3", clip = vo3 };
        voiceOvers[3] = new NamedAudioClip { name = "VO4", clip = vo4 };
        voiceOvers[4] = new NamedAudioClip { name = "VO5", clip = vo5 };
        voiceOvers[5] = new NamedAudioClip { name = "VO6", clip = vo6 };
        voiceOvers[6] = new NamedAudioClip { name = "VO7", clip = vo7 };
        voiceOvers[7] = new NamedAudioClip { name = "VO8", clip = vo8 };
        voiceOvers[8] = new NamedAudioClip { name = "VO9", clip = vo9 };
        voiceOvers[9] = new NamedAudioClip { name = "VO10", clip = vo10 };
    }
    public void ForcedPlayVO1()
    {
        BGM.clip = vo1;
        BGM.Play();
    }
    public void PlayBGM(AudioClip clip)
    {
        BGM.clip = clip;
        BGM.loop = true;
        BGM.Play();
    }
    public void PlaySound(AudioClip clip)
    {
        BGM.clip = clip;
        BGM.PlayOneShot(clip);
    }

    public void StopPlayBGM()
    {
        BGM.Stop();
    }
    public void StopPlayVO()
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
        if (clip != null && SFX != null)
        {
            SFX.clip = clip;
            SFX.Play();
        }
    }
    public void StopVO()
    {
        SFX.Stop();
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
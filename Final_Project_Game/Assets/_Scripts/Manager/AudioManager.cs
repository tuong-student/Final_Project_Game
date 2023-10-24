using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] musicSound, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioClip playOnStart;
    [SerializeField] private float timeToSwitch;
    private AudioClip switchTo;
    private float volume;

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayMusic(playOnStart, true);
    }

    public void PlayMusic(AudioClip musicToPlay, bool interup = false)
    {
        if (musicToPlay == null)
            return;
        if (interup)
        {
            musicSource.volume = 1f;
            musicSource.clip = musicToPlay;
            musicSource.Play();
        }
        else
        {
            switchTo = musicToPlay;
            StartCoroutine(SmoothSwitchAudio());
        }
    }
    public void PlaySFX(sound soundName)
    {
        Sound s = Array.Find(sfxSounds, x => x.soundType == soundName);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    private IEnumerator SmoothSwitchAudio()
    {
        volume = 1f;
        while (volume > 0f)
        {
            volume -= Time.deltaTime / timeToSwitch;
            if (volume < 0)
                volume = 0f;
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
        Play(switchTo, true);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

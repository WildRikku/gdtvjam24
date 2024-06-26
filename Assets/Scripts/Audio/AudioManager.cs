using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

[Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public float musicVolume, sfxVolume;
    [FormerlySerializedAs("stroyMode")]
    public bool storyMode = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        //PlayMusic("MenuMusic");
    }

    // How to use:
    // AudioManager.Instance.PlaySFX("name");
    // AudioManager.Instance.PlayMusic("name");
    // AudioManager.Instance.musicSource.Stop(); - Stop Music

    public void PlayMusic(string name) {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null) {
            Debug.Log("Sound Not Found");
        }
        else {
            if (musicSource.isPlaying) {
                musicSource.DOFade(musicVolume, 0.5f).SetUpdate(true).OnComplete(() => 
                {
                    musicSource.Stop();
                    musicSource.clip = s.clip;
                    musicSource.Play();
                    musicSource.DOFade(musicVolume, 0.5f).SetUpdate(true);
                });
            }
            else {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
            
        }
    }

    public void PlaySFX(string name) {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null) {
            Debug.Log("Sound Not Found");
        }
        else {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic() {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX() {
        sfxSource.mute = !sfxSource.mute;
    }

    public void SetMusicVolume(float volume) {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume) {
        sfxSource.volume = volume;
    }

    public void SetStorymode() {
        storyMode = true;
    }

    public void SetPvpmode() {
        storyMode = false;
    }
}
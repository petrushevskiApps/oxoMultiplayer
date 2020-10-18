using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxMusicSource;

    public static AudioManager Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        PlayerDataController.backgroundMusicStatusChange.AddListener(OnBgMusicStatusChange);
        PlayerDataController.sfxStatusChange.AddListener(OnSfxStatusChange);
    }

    private void OnDestroy()
    {
        PlayerDataController.backgroundMusicStatusChange.RemoveListener(OnBgMusicStatusChange);
        PlayerDataController.sfxStatusChange.RemoveListener(OnSfxStatusChange);
    }

    private void OnEnable()
    {
        OnSfxStatusChange(PlayerDataController.SfxStatus);
        OnBgMusicStatusChange(PlayerDataController.BackgroundMusicStatus);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (PlayerDataController.SfxStatus)
        {
            sfxMusicSource.clip = audioClip;
            sfxMusicSource.Play(); 
        }
    }
    private void OnSfxStatusChange(bool status)
    {
        sfxMusicSource.enabled = status;
    }

    private void OnBgMusicStatusChange(bool status)
    {
        backgroundMusicSource.enabled = status;
    }
}

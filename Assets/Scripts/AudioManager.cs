using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxMusicSource;

    protected override void RegisterListeners()
    {
        PlayerDataController.backgroundMusicStatusChange.AddListener(OnBgMusicStatusChange);
        PlayerDataController.sfxStatusChange.AddListener(OnSfxStatusChange);
    }
    
    protected override void UnregisterListeners()
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

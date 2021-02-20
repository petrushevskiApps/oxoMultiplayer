using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataController : MonoBehaviour
{
    
    public static UnityBoolEvent backgroundMusicStatusChange = new UnityBoolEvent();
    public static UnityBoolEvent sfxStatusChange = new UnityBoolEvent();
    public static UnityBoolEvent vibrationStatusChange = new UnityBoolEvent();
    
    public static UnityStringEvent UsernameChanged = new UnityStringEvent();
    public static UnityStringEvent UserIdChanged = new UnityStringEvent();
    public static UnityStringEvent UserPictureUrlChanged = new UnityStringEvent();

    public static UnityIntegerEvent playedGamesChange = new UnityIntegerEvent();
    public static UnityIntegerEvent wonGamesChange = new UnityIntegerEvent();
    public static UnityIntegerEvent lostGamesChange = new UnityIntegerEvent();

    public static bool IsUsernameSet => !string.IsNullOrEmpty(Username);
    public static string Username
    {
        get => PlayerPrefs.GetString(Keys.USER_NAME, string.Empty);
        set
        {
            PlayerPrefs.SetString(Keys.USER_NAME, value);
            UsernameChanged.Invoke(value);
        }
    }

    public static string UserId
    {
        get => PlayerPrefs.GetString(Keys.USER_ID, string.Empty);
        set
        {
            PlayerPrefs.SetString(Keys.USER_ID, value);
            UserIdChanged.Invoke(value);
        }
    }
    public static string UserPictureUrl
    {
        get => PlayerPrefs.GetString(Keys.USER_PICTURE_URL, string.Empty);
        set
        {
            PlayerPrefs.SetString(Keys.USER_PICTURE_URL, value);
            UserPictureUrlChanged.Invoke(value);
        }
    }
    
    public static int PlayedGames => PlayerPrefs.GetInt(Keys.PLAYED_GAMES, 0);

    public static void IncreasePlayedGames()
    {
        int playedGames = PlayerPrefs.GetInt(Keys.PLAYED_GAMES, 0) + 1;
        PlayerPrefs.SetInt(Keys.PLAYED_GAMES, playedGames);
        playedGamesChange.Invoke(playedGames);
    }
    
    public static int WonGames => PlayerPrefs.GetInt(Keys.WON_GAMES, 0);

    public static void IncreaseWonGames()
    {
        int wonGames = PlayerPrefs.GetInt(Keys.WON_GAMES, 0) + 1;
        PlayerPrefs.SetInt(Keys.WON_GAMES, wonGames);
        wonGamesChange.Invoke(wonGames);
    }
    
    public static int LostGames => PlayerPrefs.GetInt(Keys.LOST_GAMES, 0);

    public static void IncreaseLostGames()
    {
        int lostGames = PlayerPrefs.GetInt(Keys.LOST_GAMES, 0) + 1;
        PlayerPrefs.SetInt(Keys.LOST_GAMES, lostGames);
        lostGamesChange.Invoke(lostGames);
    }
    
    public static bool BackgroundMusicStatus
    {
        get => PlayerPrefs.GetInt(Keys.BG_MUSIC_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(Keys.BG_MUSIC_STATUS, value ? 1 : 0);
            backgroundMusicStatusChange.Invoke(value);
        }
    }
    public static bool SfxStatus
    {
        get => PlayerPrefs.GetInt(Keys.SFX_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(Keys.SFX_STATUS, value ? 1 : 0);
            sfxStatusChange.Invoke(value);
        }
    }
    public static bool VibrationStatus
    {
        get => PlayerPrefs.GetInt(Keys.VIBRATION_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(Keys.VIBRATION_STATUS, value ? 1 : 0);
            vibrationStatusChange.Invoke(value);
        }
    }

    private static Texture2D cachedUserImage;
    
    private void Awake()
    {
//        FacebookService.UsernameUpdated.AddListener(UpdateUsername);
//        FacebookService.UserIdUpdated.AddListener(UpdateUserId);
//        FacebookService.UserImageUpdate.AddListener(UpdateUserImage);
    }

    

    private void OnDestroy()
    {
//        FacebookService.UsernameUpdated.RemoveListener(UpdateUsername);
//        FacebookService.UserIdUpdated.RemoveListener(UpdateUserId);
//        FacebookService.UserImageUpdate.RemoveListener(UpdateUserImage);
    }
//
//    private void UpdateUserId(string newUserId)
//    {
//        UserId = newUserId;
//    }
//
//    private void UpdateUsername(string newUsername)
//    {
//        Username = newUsername;
//    }
//    
//    private void UpdateUserImage(string url)
//    {
//        UserPictureUrl = url;
//        Debug.Log($"URL GET IMAGE:: {url}");
//        GetUserImage();
//    }
//
//    public static Texture2D GetUserImage()
//    {
//        if (cachedUserImage != null) return cachedUserImage;
//        
//        ImageDownloader.DownloadImage(GameManager.Instance, UserPictureUrl, texture =>
//        {
//            cachedUserImage = texture;
//        });
//        
//        return cachedUserImage;
//    }
}

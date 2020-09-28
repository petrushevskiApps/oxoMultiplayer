using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataController : MonoBehaviour
{
    private const string KEY_USERNAME = "username";
    private const string KEY_PLAYED_GAMES = "playedGames";
    private const string KEY_WON_GAMES = "gamesWon";
    private const string KEY_LOST_GAMES = "gamesLost";
    private const string KEY_BG_MUSIC_STATUS = "backgroundMusic";
    private const string KEY_SFX_STATUS = "sfxStatus";
    private const string KEY_VIBRATION_STATUS = "vibrationStatus";

    public static UnityBoolEvent backgroundMusicStatusChange = new UnityBoolEvent();
    public static UnityBoolEvent sfxStatusChange = new UnityBoolEvent();
    public static UnityBoolEvent vibrationStatusChange = new UnityBoolEvent();
    
    public static UnityStringEvent usernameChanged = new UnityStringEvent();
    
    public static UnityIntegerEvent playedGamesChange = new UnityIntegerEvent();
    public static UnityIntegerEvent wonGamesChange = new UnityIntegerEvent();
    public static UnityIntegerEvent lostGamesChange = new UnityIntegerEvent();

   
    public static string Username
    {
        get => PlayerPrefs.GetString(KEY_USERNAME, string.Empty);
        set
        {
            PlayerPrefs.SetString(KEY_USERNAME, value);
            GameManager.Instance.SetUsername(value);
            usernameChanged.Invoke(value);
        }
    }

    public static int PlayedGames => PlayerPrefs.GetInt(KEY_PLAYED_GAMES, 0);

    public static void IncreasePlayedGames()
    {
        int playedGames = PlayerPrefs.GetInt(KEY_PLAYED_GAMES, 0) + 1;
        PlayerPrefs.SetInt(KEY_PLAYED_GAMES, playedGames);
        playedGamesChange.Invoke(playedGames);
    }
    
    public static int WonGames => PlayerPrefs.GetInt(KEY_WON_GAMES, 0);

    public static void IncreaseWonGames()
    {
        int wonGames = PlayerPrefs.GetInt(KEY_WON_GAMES, 0) + 1;
        PlayerPrefs.SetInt(KEY_WON_GAMES, wonGames);
        wonGamesChange.Invoke(wonGames);
    }
    
    public static int LostGames => PlayerPrefs.GetInt(KEY_LOST_GAMES, 0);

    public static void IncreaseLostGames()
    {
        int lostGames = PlayerPrefs.GetInt(KEY_LOST_GAMES, 0) + 1;
        PlayerPrefs.SetInt(KEY_LOST_GAMES, lostGames);
        lostGamesChange.Invoke(lostGames);
    }
    
    public static bool BackgroundMusicStatus
    {
        get => PlayerPrefs.GetInt(KEY_BG_MUSIC_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(KEY_BG_MUSIC_STATUS, value ? 1 : 0);
            backgroundMusicStatusChange.Invoke(value);
        }
    }
    public static bool SfxStatus
    {
        get => PlayerPrefs.GetInt(KEY_SFX_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(KEY_SFX_STATUS, value ? 1 : 0);
            sfxStatusChange.Invoke(value);
        }
    }
    public static bool VibrationStatus
    {
        get => PlayerPrefs.GetInt(KEY_VIBRATION_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(KEY_VIBRATION_STATUS, value ? 1 : 0);
            vibrationStatusChange.Invoke(value);
        }
    }

    public class UnityBoolEvent : UnityEvent<bool>
    {
    }
    public class UnityIntegerEvent : UnityEvent<int>
    {
    }
    public class UnityStringEvent : UnityEvent<string>
    {
    }
}

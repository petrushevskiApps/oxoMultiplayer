using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataController : MonoBehaviour
{
    
    public static UnityBoolEvent backgroundMusicStatusChange = new UnityBoolEvent();
    public static UnityBoolEvent sfxStatusChange = new UnityBoolEvent();
    public static UnityBoolEvent vibrationStatusChange = new UnityBoolEvent();
    
    public static UnityStringEvent usernameChanged = new UnityStringEvent();
    
    public static UnityIntegerEvent playedGamesChange = new UnityIntegerEvent();
    public static UnityIntegerEvent wonGamesChange = new UnityIntegerEvent();
    public static UnityIntegerEvent lostGamesChange = new UnityIntegerEvent();

   
    public static string Username
    {
        get => PlayerPrefs.GetString(Keys.KEY_USERNAME, string.Empty);
        set
        {
            PlayerPrefs.SetString(Keys.KEY_USERNAME, value);
            GameManager.Instance.SetUsername(value);
            usernameChanged.Invoke(value);
        }
    }

    public static int PlayedGames => PlayerPrefs.GetInt(Keys.KEY_PLAYED_GAMES, 0);

    public static void IncreasePlayedGames()
    {
        int playedGames = PlayerPrefs.GetInt(Keys.KEY_PLAYED_GAMES, 0) + 1;
        PlayerPrefs.SetInt(Keys.KEY_PLAYED_GAMES, playedGames);
        playedGamesChange.Invoke(playedGames);
    }
    
    public static int WonGames => PlayerPrefs.GetInt(Keys.KEY_WON_GAMES, 0);

    public static void IncreaseWonGames()
    {
        int wonGames = PlayerPrefs.GetInt(Keys.KEY_WON_GAMES, 0) + 1;
        PlayerPrefs.SetInt(Keys.KEY_WON_GAMES, wonGames);
        wonGamesChange.Invoke(wonGames);
    }
    
    public static int LostGames => PlayerPrefs.GetInt(Keys.KEY_LOST_GAMES, 0);

    public static void IncreaseLostGames()
    {
        int lostGames = PlayerPrefs.GetInt(Keys.KEY_LOST_GAMES, 0) + 1;
        PlayerPrefs.SetInt(Keys.KEY_LOST_GAMES, lostGames);
        lostGamesChange.Invoke(lostGames);
    }
    
    public static bool BackgroundMusicStatus
    {
        get => PlayerPrefs.GetInt(Keys.KEY_BG_MUSIC_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(Keys.KEY_BG_MUSIC_STATUS, value ? 1 : 0);
            backgroundMusicStatusChange.Invoke(value);
        }
    }
    public static bool SfxStatus
    {
        get => PlayerPrefs.GetInt(Keys.KEY_SFX_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(Keys.KEY_SFX_STATUS, value ? 1 : 0);
            sfxStatusChange.Invoke(value);
        }
    }
    public static bool VibrationStatus
    {
        get => PlayerPrefs.GetInt(Keys.KEY_VIBRATION_STATUS, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(Keys.KEY_VIBRATION_STATUS, value ? 1 : 0);
            vibrationStatusChange.Invoke(value);
        }
    }

    
}

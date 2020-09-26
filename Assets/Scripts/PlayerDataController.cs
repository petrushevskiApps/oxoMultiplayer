using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    public const string KEY_USERNAME = "username";
    public const string KEY_PLAYED_GAMES = "playedGames";
    public const string KEY_WON_GAMES = "gamesWon";
    public const string KEY_LOST_GAMES = "gamesLost";
    public const string KEY_BG_MUSIC_STATUS = "backgroundMusic";
    public const string KEY_SFX_STATUS = "sfxStatus";
    public const string KEY_VIBRATION_STATUS = "vibrationStatus";

    public static string Username
    {
        get => PlayerPrefs.GetString(KEY_USERNAME, string.Empty);
        set => PlayerPrefs.SetString(KEY_USERNAME, value);
    }
    public static int PlayedGames
    {
        get => PlayerPrefs.GetInt(KEY_PLAYED_GAMES, -1);
        set => PlayerPrefs.SetInt(KEY_PLAYED_GAMES, value);
    }
    public static int WonGames
    {
        get => PlayerPrefs.GetInt(KEY_WON_GAMES, 0);
        set => PlayerPrefs.SetInt(KEY_WON_GAMES, value);
    }
    public static int LostGames
    {
        get => PlayerPrefs.GetInt(KEY_LOST_GAMES, 0);
        set => PlayerPrefs.SetInt(KEY_LOST_GAMES, value);
    }
    public static bool BackgroundMusicStatus
    {
        get => PlayerPrefs.GetInt(KEY_BG_MUSIC_STATUS, 1) == 1;
        set => PlayerPrefs.SetInt(KEY_BG_MUSIC_STATUS, value ? 1 : 0);
    }
    public static bool SfxStatus
    {
        get => PlayerPrefs.GetInt(KEY_SFX_STATUS, 1) == 1;
        set => PlayerPrefs.SetInt(KEY_SFX_STATUS, value ? 1 : 0);
    }
    public static bool VibrationStatus
    {
        get => PlayerPrefs.GetInt(KEY_VIBRATION_STATUS, 1) == 1;
        set => PlayerPrefs.SetInt(KEY_VIBRATION_STATUS, value ? 1 : 0);
    }
}

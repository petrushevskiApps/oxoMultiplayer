using UnityEngine;
using System.Collections;

public static class Vibration
{

    private static long[] weakPattern = { 0, 100, 0 };
    private static long[] mildPattern = { 0, 200, 0 };
    private static long[] strongPattern = { 0, 500, 0 };


#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate");
        }
        else
        {
            Handheld.Vibrate();
        }
    }


    public static void Vibrate(long milliseconds)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void VibrateWeak(int repeat = -1)
    {
        if(PlayerDataController.VibrationStatus)
            Vibrate(weakPattern, repeat);
    }
    public static void VibrateMild(int repeat = -1)
    {
        if (PlayerDataController.VibrationStatus)
            Vibrate(mildPattern, repeat);
    }
    public static void VibrateStrong(int repeat = -1)
    {
        if (PlayerDataController.VibrationStatus)
            Vibrate(strongPattern, repeat);
    }

    public static bool HasVibrator()
    {
        return IsAndroid();
    }

    public static void Cancel()
    {
        if (IsAndroid())
        {
            vibrator.Call("cancel");
        }
            
    }

    private static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}
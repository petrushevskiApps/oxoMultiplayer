using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo.Utilities;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Events;

public class FacebookService : MonoBehaviour
{
    public UnityFacebookLoggedEvent LoginCompleted = new UnityFacebookLoggedEvent();
    
    public static UnityStringEvent UsernameUpdated = new UnityStringEvent();
    public static UnityStringEvent UserIdUpdated = new UnityStringEvent();
    public static UnityStringEvent UserImageUpdate = new UnityStringEvent();
    
    public const string USER_ID = "id";
    public const string USER_NAME = "name";
    public const string USER_IMG = "picture";
    
    private Dictionary<string, object> userData = new Dictionary<string, object>();

    public string Username
    {
        get => userData[USER_NAME].ToString();
        private set
        {
            if(userData.ContainsKey(USER_NAME) && Username == value) return;
            userData[USER_NAME] = value;
            UsernameUpdated.Invoke(value);
        }
    }
    public string UserId
    {
        get => userData[USER_ID].ToString();
        private set
        {
            if(userData.ContainsKey(USER_ID) && UserId == value) return;
            userData[USER_ID] = value;
            UserIdUpdated.Invoke(value);
        }
    }
    public string UserPicture
    {
        get => userData[USER_IMG].ToString();
        private set
        {
            if(userData.ContainsKey(USER_IMG) && UserPicture == value) return;
            userData[USER_IMG] = value;
            UserImageUpdate.Invoke(value);
        }
    }
    
    public void Initialize()
    {
        if (!FB.IsInitialized) 
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        } 
        else 
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    public void LogIn()
    {
        var perms = new List<string>(){"gaming_profile", "email"};
        
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        else OnSuccessfulLogin();
    }
    
    private void InitCallback ()
    {
        if (FB.IsInitialized) 
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
        } 
        else 
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
    
    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    
    

    private void AuthCallback (ILoginResult result) 
    {
        if (FB.IsLoggedIn) 
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            OnSuccessfulLogin();
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions) 
            {
                Debug.Log(perm);
            }
        } 
        else 
        {
            Debug.Log("User cancelled login");
        }
    }
    
    private void OnSuccessfulLogin()
    {
        if (FB.IsInitialized && FB.IsLoggedIn)
        {
            FB.API($"me?fields={USER_ID},{USER_NAME},{USER_IMG}", HttpMethod.GET, SetUserData, new Dictionary<string, string>());
            LoginCompleted.Invoke(AccessToken.CurrentAccessToken.TokenString, AccessToken.CurrentAccessToken.UserId);
        }
        
    }

    private void SetUserData(IGraphResult result)
    {
        Debug.Log($"RESULT:: {result.RawResult}");
        UserId = result.ResultDictionary[USER_ID].ToString();
        Username = result.ResultDictionary[USER_NAME].ToString();

        Dictionary<string, object> imageData = (Dictionary<string, object>) result.ResultDictionary[USER_IMG];
        UserPicture = ((Dictionary<string, object>)imageData["data"])["url"].ToString();

        
    }

    public class UnityFacebookLoggedEvent : UnityEvent<string, string>
    {
        
    }
}

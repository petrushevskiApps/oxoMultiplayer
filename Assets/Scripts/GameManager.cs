using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public FacebookService FacebookService { get; private set; }
    public bool IsApplicationQuiting { get; private set; }

    protected override void RegisterListeners()
    {
        FacebookService = GetComponent<FacebookService>();
        FacebookService.Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    protected override void UnregisterListeners()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (PhotonNetwork.InRoom)
        {
            UIManager.Instance.OpenScreen<UIGameScreen>();
        }
        else
        {
            if (PlayerDataController.IsUsernameSet)
            {
                UIManager.Instance.OpenScreen<UIMainScreen>();
            }
            else UIManager.Instance.OpenScreen<UIUsernameScreen>();
        }
    }

    private void OnApplicationQuit()
    {
        IsApplicationQuiting = true;
    }
    
    
}

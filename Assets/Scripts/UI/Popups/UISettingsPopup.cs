using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.menumanager;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsPopup : UIPopup
{
    [SerializeField] private GameObject musicGameObject;
    [SerializeField] private GameObject sfxGameObject;
    [SerializeField] private GameObject vibrationGameObject;
    [SerializeField] private UIButton closeButton;
    
    private UIToggle musicToggle;
    private UIToggle sfxToggle;
    private UIToggle vibrationToggle;
    
    private void Awake()
    {
        musicGameObject.GetComponent<Button>().onClick.AddListener(MusicToggle);
        sfxGameObject.GetComponent<Button>().onClick.AddListener(SfxToggle);
        vibrationGameObject.GetComponent<Button>().onClick.AddListener(VibrationToggle);
        
        closeButton.onClick.AddListener(OnBackButtonPressed);
        
        musicToggle = musicGameObject.GetComponent<UIToggle>();
        musicToggle.ToggleStatus = PlayerDataController.BackgroundMusicStatus;
        
        sfxToggle = sfxGameObject.GetComponent<UIToggle>();
        sfxToggle.ToggleStatus = PlayerDataController.SfxStatus;
        
        vibrationToggle = vibrationGameObject.GetComponent<UIToggle>();
        vibrationToggle.ToggleStatus = PlayerDataController.VibrationStatus;
    }

    private void VibrationToggle()
    {
        bool status = PlayerDataController.VibrationStatus;
        PlayerDataController.VibrationStatus = !status;
        vibrationToggle.ToggleStatus = !status;
    }

    private void SfxToggle()
    {
        bool status = PlayerDataController.SfxStatus;
        PlayerDataController.SfxStatus = !status;
        sfxToggle.ToggleStatus = !status;
    }
    
    private void MusicToggle()
    {
        bool status = PlayerDataController.BackgroundMusicStatus;
        PlayerDataController.BackgroundMusicStatus = !status;
        musicToggle.ToggleStatus = !status;
    }
}

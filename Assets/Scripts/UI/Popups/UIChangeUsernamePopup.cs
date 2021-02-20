using System.Collections;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeUsernamePopup : UIPopup
{
    [Header("Buttons")]
    [SerializeField] private UIButton saveBtn;
    [SerializeField] private UIButton closeBtn;
    
    [Header("Inputs")]
    [SerializeField] private InputField inputField;
    
    
    private void Awake()
    {
        saveBtn.interactable = false;
        saveBtn.onClick.AddListener(SaveChanges);
        closeBtn.onClick.AddListener(OnBackButtonPressed);
        inputField.onValueChanged.AddListener(EnableSaveButton);
    }
    
    private void OnDestroy()
    {
        saveBtn.onClick.RemoveListener(SaveChanges);
        closeBtn.onClick.RemoveListener(OnBackButtonPressed);
        inputField.onValueChanged.RemoveListener(EnableSaveButton);
    }
    
    private void OnEnable()
    {
        SetDefaultUsername();
    }

    private void SaveChanges()
    {
        if (inputField == null) return;
        
        string userName = inputField.text;

        if (string.IsNullOrEmpty(userName)) return;
        
        PlayerDataController.Username = userName;
        OnBackButtonPressed();
    }

    private void SetDefaultUsername()
    {
        if (inputField!=null)
        {
            inputField.text = PlayerDataController.Username;
        }
    }
        
    private void EnableSaveButton(string userName)
    {
        saveBtn.interactable = !string.IsNullOrEmpty(userName);
    }
}

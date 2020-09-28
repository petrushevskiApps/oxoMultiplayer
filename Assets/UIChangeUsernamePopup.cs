using System.Collections;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeUsernamePopup : UIPopup
{
    [SerializeField] private Button saveBtn;
    [SerializeField] private InputField inputField;
    [SerializeField] private Button closeBtn;
    
    private void Awake()
    {
        saveBtn.interactable = false;
        saveBtn.onClick.AddListener(SaveChanges);
        closeBtn.onClick.AddListener(Close);
        inputField.onValueChanged.AddListener(EnableSaveButton);
    }
    
    private void OnDestroy()
    {
        saveBtn.onClick.RemoveListener(SaveChanges);
        closeBtn.onClick.RemoveListener(Close);
        inputField.onValueChanged.RemoveListener(EnableSaveButton);
    }
    
    private void Start()
    {
        SetDefaultUsername();
    }

    private void SaveChanges()
    {
        if (inputField != null)
        {
            string userName = inputField.text;
                
            if (!string.IsNullOrEmpty(userName))
            {
                PlayerDataController.Username = userName;
                Close();
            }
        }
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
        saveBtn.interactable = true;
    }
}

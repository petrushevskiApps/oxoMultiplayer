using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using UnityEngine;
using UnityEngine.UI;

public class UICreateRoom : MonoBehaviour
{
    private Button createRoomBtn;
    [SerializeField] private InputField inputField;
    private void Awake()
    {
        createRoomBtn = GetComponent<Button>();
        createRoomBtn.onClick.AddListener(CreateRoom);
    }

    private void OnDestroy()
    {
        createRoomBtn.onClick.RemoveListener(CreateRoom);
    }

    private void CreateRoom()
    {
        GameManager.Instance.NetworkManager.CreateRoom(inputField.text);
    }
}

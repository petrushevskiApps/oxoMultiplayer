using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyTitle : MonoBehaviour
{
    [SerializeField] private Text titleText;

    private void Awake()
    {
        SetLobbyTitle();
    }

    private void SetLobbyTitle()
    {
        titleText.text = GetRoomName();
    }

    private string GetRoomName()
    {
        return PhotonNetwork.CurrentRoom.Name;
    }
}

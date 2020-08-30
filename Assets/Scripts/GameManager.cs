using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if (MatchController.LocalInstance == null && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("MatchController", Vector3.zero, Quaternion.identity);
        }
    }
}

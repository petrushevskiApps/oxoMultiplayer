using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using Photon.Pun;
using UnityEngine;

public class UI_PlayOfflineButton : UI_PlayButton
{
    protected override void OnPlayClicked()
    {
        base.OnPlayClicked();
        
        PhotonNetwork.Disconnect();
        NetworkManager.Instance.ConnectionController.PlayOffline = true;
        NetworkManager.Instance.CreateRoom();
    }
}

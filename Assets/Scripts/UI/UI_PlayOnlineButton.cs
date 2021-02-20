using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using UnityEngine;

public class UI_PlayOnlineButton : UI_PlayButton
{
    protected override void OnPlayClicked()
    {
        base.OnPlayClicked();
        NetworkManager.Instance.ConnectionController.PlayOffline = false;
        NetworkManager.Instance.JoinRandomRoom();
    }
}

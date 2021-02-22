using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;
using UnityEngine;

public class UI_PlayOfflineButton : UI_PlayButton
{
    protected override void OnPlayClicked()
    {
        base.OnPlayClicked();

        NetworkManager.Instance.ConnectionController.SetOfflineMode(true, () => NetworkManager.Instance.CreateRoom());
       
    }
}

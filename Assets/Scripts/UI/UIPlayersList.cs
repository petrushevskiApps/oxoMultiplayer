using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayersList : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playersListParent;
    [SerializeField] private GameObject playerUsernamePrefab;
    
    private Dictionary<string, GameObject> playersDictionary = new Dictionary<string, GameObject>();

    
    public override void OnEnable()
    {
        base.OnEnable();
        NetworkManager.PlayerEnteredRoom.AddListener(AddPlayerToUiList);
        NetworkManager.PlayerExitedRoom.AddListener(RemovePlayerFromUi);
        SetPlayersList();
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        NetworkManager.PlayerEnteredRoom.RemoveListener(AddPlayerToUiList);
        NetworkManager.PlayerExitedRoom.RemoveListener(RemovePlayerFromUi);
        
        foreach (KeyValuePair<string, GameObject> keyValuePair in playersDictionary)
        {
            Destroy(keyValuePair.Value);
        }
        playersDictionary.Clear();
    }

    private void SetPlayersList()
    {
        foreach(Photon.Realtime.Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            AddPlayerToUiList(player);
        }
    }

    private void AddPlayerToUiList(Photon.Realtime.Player player)
    {
        if (playersDictionary.ContainsKey(player.UserId)) return;
        
        GameObject playerRow = Instantiate(playerUsernamePrefab, playersListParent.transform);
        playersDictionary.Add(player.UserId, playerRow);
        playerRow.GetComponent<PlayerRow>()?.SetPlayerName(player.NickName);

    }

    private void RemovePlayerFromUi(Photon.Realtime.Player player)
    {
        if (playersDictionary.ContainsKey(player.UserId))
        {
            Destroy(playersDictionary[player.UserId]);
            playersDictionary.Remove(player.UserId);
        }
    }
    
    
}

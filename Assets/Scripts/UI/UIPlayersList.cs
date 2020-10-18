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
        RoomController.PlayerEnteredRoom.AddListener(AddPlayerToUiList);
        RoomController.PlayerExitedRoom.AddListener(RemovePlayerFromUi);
        
        SetPlayersList();
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        
        RoomController.PlayerEnteredRoom.RemoveListener(AddPlayerToUiList);
        RoomController.PlayerExitedRoom.RemoveListener(RemovePlayerFromUi);
        
        foreach (KeyValuePair<string, GameObject> keyValuePair in playersDictionary)
        {
            Destroy(keyValuePair.Value);
        }
        
        playersDictionary.Clear();
    }

    private void SetPlayersList()
    {
        foreach (NetworkPlayer player in NetworkManager.Instance.RoomController.GetPlayersInRoom)
        {
            AddPlayerToUiList(player);
        }
    }

    private void AddPlayerToUiList(NetworkPlayer player)
    {
        if (playersDictionary.ContainsKey(player.UserId)) return;
        
        GameObject playerRow = Instantiate(playerUsernamePrefab, playersListParent.transform);
        playersDictionary.Add(player.UserId, playerRow);
        
        PlayerRow row = playerRow.GetComponent<PlayerRow>();
        row.SetPlayerName(player.Nickname);
        row.SetToggle(player.IsReady);
        row.RegisterToggleEvent(player.PlayerStatusChange);
    }

    private void RemovePlayerFromUi(NetworkPlayer player)
    {
        if (playersDictionary.ContainsKey(player.UserId))
        {
            GameObject playerRow = playersDictionary[player.UserId];
            playerRow.GetComponent<PlayerRow>().UnregisterEvent(player.PlayerStatusChange);
            Destroy(playerRow);
            playersDictionary.Remove(player.UserId);
        }
    }
    
    
}

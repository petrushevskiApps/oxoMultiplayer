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

    private void Awake()
    {
        RoomController.PlayerEnteredRoom.AddListener(AddPlayerToUiList);
        RoomController.PlayerExitedRoom.AddListener(RemovePlayerFromUi);
    }

    private void OnDestroy()
    {
        RoomController.PlayerEnteredRoom.RemoveListener(AddPlayerToUiList);
        RoomController.PlayerExitedRoom.RemoveListener(RemovePlayerFromUi);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        
        SetPlayersList();
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        
        
        
//        foreach (KeyValuePair<string, GameObject> keyValuePair in playersDictionary)
//        {
//            Destroy(keyValuePair.Value);
//        }
//        
//        playersDictionary.Clear();
    }

    private void SetPlayersList()
    {
        foreach (NetworkPlayer player in RoomController.Instance.PlayersInRoom)
        {
            AddPlayerToUiList(player);
        }
    }

    private void AddPlayerToUiList(NetworkPlayer player)
    {
        if (playersDictionary.ContainsKey(player.UserId)) return;
        
        GameObject playerRow = Instantiate(playerUsernamePrefab, playersListParent.transform);
        
        playerRow.GetComponent<PlayerRow>().SetupRow(player);
       
        playerRow.SetActive(true);
        
        playersDictionary.Add(player.UserId, playerRow);
    }

    private void RemovePlayerFromUi(NetworkPlayer player)
    {
        if (!playersDictionary.ContainsKey(player.UserId)) return;
        
        Destroy(playersDictionary[player.UserId]);
        playersDictionary.Remove(player.UserId);
    }
    
    
}

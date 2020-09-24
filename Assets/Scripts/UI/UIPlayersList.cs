using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayersList : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playersListParent;

    [SerializeField] private GameObject playerUsernamePrefab;

    private List<Photon.Realtime.Player> players;

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        players.Add(newPlayer);
        AddPlayerToUIList(newPlayer);
    }

    private void Start()
    {
        SetPlayersList();
    }

    private void SetPlayersList()
    {
        Dictionary<int, Photon.Realtime.Player> keyValuePairs = PhotonNetwork.CurrentRoom.Players;

        players = new List<Photon.Realtime.Player>();
        foreach(Photon.Realtime.Player player in keyValuePairs.Values)
        {
            players.Add(player);
        }
        
        players.ForEach(action: x => AddPlayerToUIList(x));
    }

    private void AddPlayerToUIList(Photon.Realtime.Player player)
    {
        GameObject playerName = Instantiate(playerUsernamePrefab, playersListParent.transform);
        playerName.GetComponent<Text>().text = player.NickName;
    }
}

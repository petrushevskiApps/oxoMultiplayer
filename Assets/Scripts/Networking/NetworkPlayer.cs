
using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class NetworkPlayer
{
    public UnityBoolEvent PlayerStatusChange = new UnityBoolEvent();
    public UnityIntegerEvent PlayerScoreUpdated = new UnityIntegerEvent();
    
    public int PlayerId { get; }
    public string Nickname => player.NickName;
    public string UserId => player.UserId;
    public TileType PlayerSymbol { get; }

    private bool networkUpdate;
    private bool isLocalReady;
    public bool IsReady
    {
        get
        {
            if (player.IsMasterClient) return true;
            else
            {
                object isReady = GetPlayerProperty(Keys.PLAYER_READY_KEY);
                if (isReady != null) return (bool) isReady;
                else return false;
            }
        }
        private set
        {
            if (isLocalReady != value)
            {
                isLocalReady = value;
                PlayerStatusChange.Invoke(value);
                if(player.IsLocal && !networkUpdate) SetPlayerProperty(Keys.PLAYER_READY_KEY, value);
                
            }
            networkUpdate = false;
        }
    }
    

    private int localScore;
    
    public int Score
    {
        get => player.IsLocal ? localScore : (int)GetPlayerProperty(Keys.PLAYER_MATCH_SCORE);
        set
        {
            if (localScore != value)
            {
                Debug.Log("T1:: " + Nickname + "Set Score:: " + value + " Is Network Update:: " + networkUpdate);
                localScore = value;
                PlayerScoreUpdated.Invoke(localScore);
                if (player.IsLocal && !networkUpdate)
                {
                    SetPlayerProperty(Keys.PLAYER_MATCH_SCORE, value);
                }
            }
            networkUpdate = false;
        }
    }
    
    private Player player;
    private int playerTurnId;
    private Hashtable playerProperties;
    
    public bool IsActive()
    {
        int turn = NetworkManager.Instance.RoomController.GetRoomProperty(Keys.ROOM_TURN);
        return turn % PhotonNetwork.CurrentRoom.PlayerCount == playerTurnId;
    }
    
    
    public NetworkPlayer(Player player)
    {
        this.player = player;
        
        if (player.IsLocal)
        {
            SetupPlayerProperties();
            MatchController.MatchStarted.AddListener(OnMatchStarted);
        }
        
        PlayerId = player.ActorNumber;
        PlayerSymbol = (TileType) PlayerId;
        playerTurnId = PlayerId - 1;
        
        UpdatePlayerStatuses(player.CustomProperties);
    }

    private void OnMatchStarted()
    {
        if(player.IsLocal) Score = 0;
        IsReady = player.IsMasterClient;
    }

    public void UpdatePlayerStatuses(Hashtable statuses)
    {
        if (statuses.ContainsKey(Keys.PLAYER_READY_KEY))
        {
            networkUpdate = true;
            IsReady = (bool)statuses[Keys.PLAYER_READY_KEY];
        }
        
        if (statuses.ContainsKey(Keys.PLAYER_MATCH_SCORE))
        {
            networkUpdate = true;
            Score = (int)statuses[Keys.PLAYER_MATCH_SCORE];
        }
    }

    private void SetupPlayerProperties()
    {
        playerProperties = new Hashtable();
        playerProperties.Add(Keys.PLAYER_READY_KEY, false);
        playerProperties.Add(Keys.PLAYER_MATCH_ID, playerTurnId);
        playerProperties.Add(Keys.PLAYER_MATCH_SCORE, 0);
        player.SetCustomProperties(playerProperties);
    }
    
    public void SetPlayerProperty(string KEY, int value)
    {
        if(!player.IsLocal) return;
        
        playerProperties[KEY] = value;
        player.SetCustomProperties(playerProperties);
    }
    public object GetPlayerProperty(string KEY)
    {
        object result = 0;
        player.CustomProperties.TryGetValue(KEY, out result);
        return result;
    }
        
    public void SetPlayerProperty(string KEY, bool value)
    {
        playerProperties[KEY] = value;
        player.SetCustomProperties(playerProperties);
    }

}

using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Properties;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPlayer
{
    public readonly UnityBoolEvent ReadyStatusChanged = new UnityBoolEvent();
    public readonly UnityIntegerEvent ScoreUpdated = new UnityIntegerEvent();
    
    public INetworkProperties Properties { get; }
    
    public string Nickname => player.NickName;
    public string UserId => player.UserId;
    public TileType PlayerSymbol { get; }

    public int PlayerId
    {
        get => Properties.GetProperty<int>(Keys.PLAYER_MATCH_ID);
        private set
        {
            if(PlayerId > 0) return;
            Properties.Set(Keys.PLAYER_MATCH_ID, value).Sync();
        }
    }

    public bool IsReady
    {
        get => player.IsMasterClient || Properties.GetProperty<bool>(Keys.PLAYER_READY_KEY);
        set
        {
            if (IsReady == value) return;
            
            ReadyStatusChanged.Invoke(value);
            Properties.Set(Keys.PLAYER_READY_KEY, value).Sync();
        }
    }
    
    public int Score
    {
        get => Properties.GetProperty<int>(Keys.PLAYER_MATCH_SCORE);
        set
        {
            if (Score == value) return;
            
            ScoreUpdated.Invoke(value);
            Properties.Set(Keys.PLAYER_MATCH_SCORE, value).Sync();
            Debug.Log($"NetworkPlayer:: {Nickname}:: New Score:: {value}");
        }
    }
    
    public bool IsActive
    {
        get
        {
            int turn = MatchController.LocalInstance.Turn;
            return turn % PhotonNetwork.CurrentRoom.PlayerCount == (PlayerId - 1);
        }
    }

    public bool IsConnected => !player.IsInactive;
    
    private Player player;
    private bool isActive;

    
    public NetworkPlayer(Player player)
    {
        this.player = player;
        Properties = new PlayerProperties(player);
        Properties.Set(Keys.PLAYER_READY_KEY, player.IsMasterClient)
                    .Set(Keys.PLAYER_MATCH_SCORE, 0)
                    .Sync();
        
        if (player.IsLocal)
        {
            MatchController.MatchStartSynced.AddListener(OnMatchStarted);
            MatchController.MatchEnd.AddListener(OnMatchEnded);
        }
        
        PlayerId = player.ActorNumber;
        PlayerSymbol = (TileType) player.ActorNumber;
    }
    
    private void OnMatchStarted()
    {
        IsReady = player.IsMasterClient;
    }
    private void OnMatchEnded(bool arg0)
    {
        Score = 0;
    }
    
    public void UpdatePlayerStatuses(Hashtable properties)
    {
        Properties.Updated();
        
        if (properties.ContainsKey(Keys.PLAYER_READY_KEY))
        {
            ReadyStatusChanged.Invoke((bool)properties[Keys.PLAYER_READY_KEY]);
        }
        
        if (properties.ContainsKey(Keys.PLAYER_MATCH_SCORE))
        {
            ScoreUpdated.Invoke((int)properties[Keys.PLAYER_MATCH_SCORE]);
        }
    }


}

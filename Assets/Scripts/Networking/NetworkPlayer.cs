using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Properties;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class NetworkPlayer
{
    public readonly UnityEvent ReadyStatusChanged = new UnityEvent();
    public readonly UnityIntegerEvent ScoreUpdated = new UnityIntegerEvent();
    
    private INetworkProperties Properties { get; }
    
    public string Nickname => player.NickName;
    
    public TilePlayerSign PlayerSignSymbol { get; }

    public int PlayerId
    {
        get => Properties.GetProperty<int>(Keys.PLAYER_MATCH_ID);
        private set
        {
            if(PlayerId > 0) return;
            Properties.Set(Keys.PLAYER_MATCH_ID, value).Sync();
        }
    }

    private bool IsReady
    {
        get => player.IsMasterClient || Properties.GetProperty<bool>(Keys.PLAYER_READY_KEY);
        set
        {
            if (IsReady == value) return;
            
            ReadyStatusChanged.Invoke();
            Properties.Set(Keys.PLAYER_READY_KEY, value).Sync();
        }
    }
    
    public int Score
    {
        get => Properties.GetProperty<int>(Keys.PLAYER_MATCH_SCORE);
        set
        {
            if (Score == value) return;
            
            Properties.Set(Keys.PLAYER_MATCH_SCORE, value).Sync();
            ScoreUpdated.Invoke(value);
            Debug.Log($"NetworkPlayer:: {Nickname}:: New Score:: {value}");
        }
    }
    
    public bool IsActive
    {
        get
        {
            int turn = MatchController.LocalInstance.Turn;
            return (turn % PhotonNetwork.CurrentRoom.PlayerCount) == (PlayerId - 1);
        }
    }

    
    private Player player;
    private bool isActive;

    public NetworkPlayer(Player player)
    {
        this.player = player;
        Properties = new PlayerProperties(player);
        SetDefaultProperties(player.IsMasterClient);
        
        if (player.IsLocal)
        {
            MatchController.MatchStartSynced.AddListener(OnMatchStarted);
            MatchController.MatchEnd.AddListener(OnMatchEnded);
        }
        
        PlayerId = player.ActorNumber;
        PlayerSignSymbol = (TilePlayerSign) player.ActorNumber;
    }
    
    ~ NetworkPlayer()
    {
        SetDefaultProperties(false);
        
        if (player.IsLocal)
        {
            MatchController.MatchStartSynced.RemoveListener(OnMatchStarted);
            MatchController.MatchEnd.RemoveListener(OnMatchEnded);
        }
    }

    private void SetDefaultProperties(bool isReady)
    {
        Properties.Set(Keys.PLAYER_READY_KEY, isReady)
                  .Set(Keys.PLAYER_MATCH_SCORE, 0)
                  .Sync();
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
        if (properties.ContainsKey(Keys.PLAYER_READY_KEY))
        {
            Properties.Updated(Keys.PLAYER_READY_KEY);
        }
        
        if (properties.ContainsKey(Keys.PLAYER_MATCH_SCORE))
        {
            Properties.Updated(Keys.PLAYER_MATCH_SCORE);
        }
    }


}

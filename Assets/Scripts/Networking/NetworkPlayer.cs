using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Properties;
using com.petrushevskiapps.Oxo.Utilities;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPlayer
{
    public readonly UnityBoolEvent ReadyStatusChanged = new UnityBoolEvent();
    public readonly UnityBoolEvent ActiveStatusChanged = new UnityBoolEvent();
    public readonly UnityIntegerEvent ScoreUpdated = new UnityIntegerEvent();
    
    public INetworkProperties Properties { get; }
    public int PlayerId { get; }
    public string Nickname => player.NickName;
    public string UserId => player.UserId;
    public TileType PlayerSymbol { get; }

    public bool IsReady
    {
        get => player.IsMasterClient || Properties.GetProperty<bool>(Keys.PLAYER_READY_KEY);
        private set
        {
            if (IsReady == value) return;
            
            ReadyStatusChanged.Invoke(value);
            Properties.Set(Keys.PLAYER_READY_KEY, value).Update();
        }
    }
    

    public int Score
    {
        get => Properties.GetProperty<int>(Keys.PLAYER_MATCH_SCORE);
        set
        {
            if (Score == value) return;
            
            ScoreUpdated.Invoke(value);
            Properties.Set(Keys.PLAYER_MATCH_SCORE, value).Update();
            Debug.Log($"NetworkPlayer:: {Nickname}:: New Score:: {value}");
        }
    }
    
    public bool IsActive
    {
        get => isActive;
        private set
        {
            if (isActive == value) return;
            isActive = value;
            ActiveStatusChanged.Invoke(isActive);
        }
    }
    
    private Player player;
    private int playerTurnId;
    private bool isActive;

    
    public NetworkPlayer(Player player)
    {
        this.player = player;
        
        if (player.IsLocal)
        {
            MatchController.MatchStarted.AddListener(OnMatchStarted);
            MatchController.MatchEnded.AddListener(OnMatchEnded);
        }
        
        RoomController.TurnChanged.AddListener(SetActiveStatus);

        PlayerId = player.ActorNumber;
        PlayerSymbol = (TileType) PlayerId;
        playerTurnId = PlayerId - 1;
        Properties = new PlayerProperties(player, playerTurnId);
        
        UpdatePlayerStatuses(player.CustomProperties);
    }
    
    private void OnMatchStarted()
    {
        if(player.IsLocal) Score = 0;
    }
    private void OnMatchEnded(bool arg0)
    {
        IsReady = player.IsMasterClient;
    }
    private void SetActiveStatus(int turn)
    {
        IsActive = turn % PhotonNetwork.CurrentRoom.PlayerCount == playerTurnId;
    }
    
    public void UpdatePlayerStatuses(Hashtable statuses)
    {
        if (statuses.ContainsKey(Keys.PLAYER_READY_KEY))
        {
            ReadyStatusChanged.Invoke((bool)statuses[Keys.PLAYER_READY_KEY]);
        }
        
        if (statuses.ContainsKey(Keys.PLAYER_MATCH_SCORE))
        {
            ScoreUpdated.Invoke((int)statuses[Keys.PLAYER_MATCH_SCORE]);
        }
    }


}

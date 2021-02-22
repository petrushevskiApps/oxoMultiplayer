using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Properties;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class NetworkPlayer : IPlayer
{
    private readonly UnityIntegerEvent ScoreUpdated = new UnityIntegerEvent();

    private INetworkProperties Properties { get; }
    private readonly Player player;
    private bool isLocal;
    public NetworkPlayer(int playerId, string playerNickname)
    {
        Properties = new PlayerProperties();
        SetDefaultProperties();
        SetId(playerId);
        Sign = (TilePlayerSign)playerId;
        Nickname = playerNickname;
        isLocal = true;
        
    }
    public NetworkPlayer(Player player)
    {
        this.player = player;
        Properties = new PlayerProperties(player);
        SetDefaultProperties();
        SetId(player.ActorNumber);
        Sign = (TilePlayerSign)player.ActorNumber;
        Nickname = player.NickName;
        isLocal = player.IsLocal;
    }
    ~NetworkPlayer()
    {
        SetDefaultProperties();

        if (isLocal)
        {
            MatchController.MatchEnd.RemoveListener(OnMatchEnded);
        }
    }

    public void Init()
    {
        if (isLocal)
        {
            MatchController.MatchEnd.AddListener(OnMatchEnded);
        }
    }

    

    public bool IsActive()
    {
        int turn = MatchController.LocalInstance.Turn;
        return (turn % NetworkManager.Instance.RoomController.PlayersInRoom.Count) == (GetId() - 1);
    }

    public TilePlayerSign Sign { get; }

    public string Nickname { get; }

    public int GetId()
    {
        return Properties.GetProperty<int>(Keys.PLAYER_MATCH_ID);
    }

    private void SetId(int playerId)
    {
        if(GetId() > 0) return;
        Properties.Set(Keys.PLAYER_MATCH_ID, playerId).Sync();
    }

    public int GetScore()
    {
        return Properties.GetProperty<int>(Keys.PLAYER_MATCH_SCORE);
    }

    public void IncrementScore()
    {
        SetScore(GetScore() + 1);
    }

    public void SetScore(int score)
    {
        Properties.Set(Keys.PLAYER_MATCH_SCORE, score).Sync();
        ScoreUpdated.Invoke(score);
        Debug.Log($"NetworkPlayer:: {Nickname}:: New Score:: {score}");
    }

    
    private void SetDefaultProperties()
    {
        Properties.Set(Keys.PLAYER_MATCH_ID, 0).Sync();
        Properties.Set(Keys.PLAYER_MATCH_SCORE, 0).Sync();
    }

    private void OnMatchEnded(bool arg0)
    {
        SetScore(0);
    }
    
    public void UpdatePlayerStatuses(Hashtable properties)
    {
        if (properties.ContainsKey(Keys.PLAYER_MATCH_SCORE))
        {
            ScoreUpdated.Invoke((int)properties[Keys.PLAYER_MATCH_SCORE]);
            Properties.Updated(Keys.PLAYER_MATCH_SCORE);
        }
    }

    
    public void RegisterScoreListener(UnityAction<int> listener)
    {
        ScoreUpdated.AddListener(listener);
    }

    public void UnregisterScoreListener(UnityAction<int> listener)
    {
        ScoreUpdated.RemoveListener(listener);
    }


}

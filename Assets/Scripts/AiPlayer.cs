using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using UnityEngine;
using UnityEngine.Events;

public class AiPlayer : IPlayer
{
    private readonly UnityIntegerEvent ScoreUpdated = new UnityIntegerEvent();
    
    private AiBrain aiBrain;
    private int id;
    private int score;
    private string nickname;

    public AiPlayer(int aiSign)
    {
        MatchController.MatchStart.AddListener(SetupAiBrain);
        MatchController.RoundStarted.AddListener(OnRoundStarted);
        MatchController.TurnChanged.AddListener(OnTurnChanged);
        
        SetId(aiSign);
        
        //TODO:: Generate random nicknames for AI player
        nickname = "AI-Player";
    }

    ~AiPlayer()
    {
        MatchController.MatchStart.RemoveListener(SetupAiBrain);
        MatchController.RoundStarted.RemoveListener(OnRoundStarted);
        MatchController.TurnChanged.RemoveListener(OnTurnChanged);
    }

    private void SetupAiBrain()
    {
        aiBrain = new AiBrain();
        int playerSign = NetworkManager.Instance.RoomController.LocalPlayer.GetId();
        aiBrain.Initialize(id, playerSign, MatchController.LocalInstance.WinCondition);
    }
    
    private void OnRoundStarted()
    {
        MakeMove(MatchController.LocalInstance.Board.TilesTable);
    }
    
    private void OnTurnChanged(int turn)
    {
        MakeMove(MatchController.LocalInstance.Board.TilesTable);
    }

    private void MakeMove(Tile[,] table)
    {
        Timer.Start(GameManager.Instance, "AiPlayerStart", 1f, () =>
        {
            if (IsActive())
            {
                int tileId = aiBrain.FindBestMove(table);
                table[Utilities.GetRowFromId(tileId,table), Utilities.GetColumnFromId(tileId, table)].TileClicked();
            }
        });

    }
    
    public bool IsActive()
    {
        int turn = MatchController.LocalInstance.Turn;
        return (turn % NetworkManager.Instance.RoomController.PlayersInRoom.Count) == (GetId() - 1);
    }

    public TilePlayerSign Sign => (TilePlayerSign)id;

    public string Nickname => nickname;

    public int GetId()
    {
        return id;
    }

    private void SetId(int playerId)
    {
        if(id > 0 || playerId < 0) return;
        id = playerId;
    }

    public int GetScore()
    {
        return score;
    }

    public void IncrementScore()
    {
        SetScore(GetScore() + 1);
    }

    public void SetScore(int newScore)
    {
        if(score == newScore) return;
        score = newScore;
        ScoreUpdated.Invoke(newScore);
        Debug.Log($"AiPlayer:: {Nickname}:: New Score:: {newScore}");
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

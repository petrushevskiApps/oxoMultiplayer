using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Grid;
using Photon.Pun;
using UnityEngine;

public class BoardController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GridCreator gridCreator;
    
    private Tile[,] tilesTable;
    private WinCondition winCondition;

    private void Awake()
    {
        winCondition = new WinCondition(3);
        
        MatchController.RoundEnd.AddListener(ResetBoard);
        MatchController.MatchEnd.AddListener(OnMatchEnded);
        
        SetupBoard();
    }
    
    private void OnDestroy()
    {
        MatchController.RoundEnd.RemoveListener(ResetBoard);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
    }

    
    private void SetupBoard()
    {
        int rows = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("r");
        int columns = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("c");
        
        tilesTable = gridCreator.CreateGrid<Tile>(rows, columns);

        int tileId = 0;
        
        foreach (Tile tile in tilesTable)
        {
            tile.StateChange.AddListener(TurnEnded);
            tile.Id = tileId;
            tileId++;
        }
    }

    private void ResetBoard()
    {
        foreach (Tile tile in tilesTable)
        {
            tile.SetTile();
        }
    }
    
    private void OnMatchEnded(bool arg0)
    {
        NetworkManager.Instance.ClearRpcs(photonView);
    }
    
    private void TurnEnded(int tileId)
    {
        int playerId = NetworkManager.Instance.RoomController.ActivePlayer.PlayerId;
        NetworkManager.Instance.SendRpc(photonView, RPCs.RPC_TURN_END, true, tileId, playerId);
    }
    
    [PunRPC]
    private void TurnEnd(int tileId, int playerId)
    {
        NetworkManager.Instance.RoomController.LocalRpcBufferCount++;
        
        UpdateTile(tileId, playerId);

        if (winCondition.IsRoundWon(playerId, tileId, tilesTable))
        {
            RoundEnded(playerId);
            return;
        }

        if (winCondition.IsTableFull(tilesTable))
        {
            MatchController.LocalInstance.RoundTie();
            return;
        }
        
        MatchController.LocalInstance.Turn++;
        
    }

    private void UpdateTile(int id, int playerId)
    {
        Tile tile = tilesTable[Utilities.GetRowFromId(id,tilesTable), Utilities.GetColumnFromId(id, tilesTable)];
        tile.ChangeState(playerId);
        tile.PlayerId = playerId;
        Utilities.PrintTable(tilesTable);
    }
    
    private void RoundEnded(int playerId)
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        bool isRoundWon = NetworkManager.Instance.RoomController.LocalPlayer.PlayerId == playerId;
        
        StrikeEffect(isRoundWon);

        Timer.Start(this, "RoundEndDelay", 0.5f, ()=>
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            if(isRoundWon) MatchController.LocalInstance.RoundWon();
            ResetBoard();
        });
    }
    
    
    
    private void StrikeEffect(bool isRoundWon)
    {
        List<int> winIDs = winCondition.GetWinIds();
        
        winIDs.ForEach(id =>
        {
            tilesTable[Utilities.GetRowFromId(id, tilesTable), Utilities.GetColumnFromId(id, tilesTable)].ShowEndEffect(isRoundWon);
        });
    }

    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

}

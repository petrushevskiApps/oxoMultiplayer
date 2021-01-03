using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class BoardController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Grid grid;
    
    private int[,] tilesTable;

    private WinCondition winCondition;
    
    private int rows;
    private int columns;
    
    private int xSize = 0;
    private int ySize = 0;
    
    
    private void Awake()
    {
        winCondition = new WinCondition(3);
        
        MatchController.RoundEnd.AddListener(ResetBoard);
        MatchController.MatchEnd.AddListener(OnMatchEnded);
        
        rows = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("r");
        columns = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("c");

        SetupBoard(rows,columns);
        SetTilesTable();
    }
    
    private void OnDestroy()
    {
        MatchController.RoundEnd.RemoveListener(ResetBoard);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
    }

    
    private void SetupBoard(int rows, int columns)
    {
        tilesTable = new int[rows, columns];
        grid.CreateGrid(rows, columns);
        
        int tileId = 0;
        
        grid.ForEachOfComponent<Tile>(tile =>
        {
            tile.StateChange.AddListener(TurnEnded);
            tile.Id = tileId;
            tileId++;
        });
    }

    private void ResetBoard()
    {
        SetTilesTable();
        grid.ForEachOfComponent<Tile>(tile => tile.SetTile());
    }
    
    private void SetTilesTable()
    {
        xSize = tilesTable.GetUpperBound(0);
        ySize = tilesTable.GetUpperBound(1);
        
        for (int i = 0; i <= xSize; i++)
        {
            for (int j = 0; j <= ySize; j++)
            {
                tilesTable[i,j] = 0;
            }
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
        
        ElementIndex index = new ElementIndex(tileId, xSize, ySize);
        
        UpdateTile(index, tileId, playerId);

        if (winCondition.IsRoundWon(playerId, index, tilesTable))
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

    
    
    private void UpdateTile(ElementIndex index,int id, int playerId)
    {
        grid.ElementAt<Tile>(id).ChangeState(playerId);
        tilesTable[index.Row, index.Column] = playerId;
        
        Utilities<int>.PrintTable(tilesTable);
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
        });
    }
    
    
    
    private void StrikeEffect(bool isRoundWon)
    {
        List<ElementIndex> winIndexes = winCondition.GetWinIndexes().ToList();
        
        winIndexes.ForEach(index =>
        {
            grid.ElementAt<Tile>(index.GetTileId(columns)).ShowStrikeEffect(isRoundWon);
        });
    }

    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

}

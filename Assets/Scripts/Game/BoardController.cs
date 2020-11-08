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
    
    private int[,] tilesTable = new int[maxRow, maxColumn];

    private List<Tile> tiles = new List<Tile>();
    
    private WinCondition winCondition;
    
    private static int maxRow = 3;
    private static int maxColumn = 3;
    
    private int xSize = 0;
    private int ySize = 0;
    
    
    private void Awake()
    {
        winCondition = GetComponent<WinCondition>();
        
        MatchController.RoundEnd.AddListener(ResetBoard);
        
        SetupBoardTiles();
        SetTilesTable();
    }
    private void OnDestroy()
    {
        MatchController.RoundEnd.RemoveListener(ResetBoard);
    }

    private void SetupBoardTiles()
    {
        tiles = GetComponentsInChildren<Tile>().ToList();
        
        tiles.ForEach(x =>
        {
            x.TileStateChange.AddListener(TurnEnded);
        });
    }
    
    private void ResetBoard()
    {
        SetTilesTable();
        tiles.ForEach(tile => tile.SetTile());
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
  
    private void TurnEnded(int tileId)
    {
        int playerId = RoomController.Instance.ActivePlayer.PlayerId;
        photonView.RPC("TurnEnd", RpcTarget.AllBufferedViaServer, tileId, playerId);
    }
    
    [PunRPC]
    private void TurnEnd(int tileId, int playerId)
    {
        UpdateTile(tileId, playerId);
       
        bool isWin = winCondition.CheckWinCondition(tilesTable);
        bool isTie = winCondition.CheckTie(tilesTable);
        
        
        if (isWin || isTie)
        {
            RoundEnded(playerId);
            return;
        }

        MatchController.LocalInstance.IncrementTurn();
        
    }
    
    private void UpdateTile(int id, int playerId)
    {
        tiles[id].ChangeState(playerId);
        tilesTable[id / (xSize + 1), id % (ySize + 1)] = playerId;
        PrintTable();
    }
    
    private void RoundEnded(int playerId)
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        bool isRoundWon = RoomController.Instance.LocalPlayer.PlayerId == playerId;
        
        StrikeEffect(isRoundWon);

        Timer.Start(this, "RoundEndDelay", 0.5f, ()=>
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            MatchController.LocalInstance.EndRound(isRoundWon);
        });
    }
    
    
    
    private void StrikeEffect(bool isRoundWon)
    {
        List<WinCondition.RowColumIndex> rowColumnIndex = winCondition.GetWinIndexes();
            
        rowColumnIndex.ForEach(index =>
        {
            int tileId = (index.row * maxColumn) + index.column;
            tiles[tileId].StrikeTileEffect(isRoundWon);
        });
    }

    private void PrintTable()
    {
        int printIndex = 0;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        foreach (int tile in tilesTable)
        {
            sb.Append(tile + " ");
            printIndex++;
            if (printIndex % 3 == 0 && printIndex > 0)
            {
                sb.AppendLine();
            }
        }
        Debug.Log(sb);
    }

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

}

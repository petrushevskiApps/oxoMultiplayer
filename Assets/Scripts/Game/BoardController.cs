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
        MatchController.MatchEnd.AddListener(OnMatchEnded);
        
        SetupBoardTiles();
        SetTilesTable();
    }
    private void OnDestroy()
    {
        MatchController.RoundEnd.RemoveListener(ResetBoard);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
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
       
        bool isWin = winCondition.CheckWinCondition(tilesTable);
        bool isTie = winCondition.CheckTie(tilesTable);

        if (isWin)
        {
            RoundEnded(playerId);
            return;
        }

        if (isTie)
        {
            MatchController.LocalInstance.RoundTie();
            return;
        }
        
        MatchController.LocalInstance.Turn++;
        
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

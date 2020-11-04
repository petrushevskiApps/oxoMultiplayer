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
    private TurnController turnController;
    
    private static int maxRow = 3;
    private static int maxColumn = 3;
    
    private int xSize = 0;
    private int ySize = 0;
    
    
    private void Awake()
    {
        winCondition = GetComponent<WinCondition>();
        turnController = GetComponent<TurnController>();

        MatchController.RoundEnded.AddListener(ResetBoard);
        
        SetupBoardTiles();
    }
    
    private void SetupBoardTiles()
    {
        tiles = GetComponentsInChildren<Tile>().ToList();
        
        tiles.ForEach(x =>
        {
            x.TileStateChange.AddListener(TurnEnded);
        });
    }
    
    private void OnDestroy()
    {
        MatchController.RoundEnded.RemoveListener(ResetBoard);
    }
    
    private void ResetBoard()
    {
        SetTilesTable();
        tiles.ForEach(tile => tile.SetTile());
    }
    
    private void Start()
    {
        SetTilesTable();
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
        photonView.RPC("TurnEnd", RpcTarget.AllBuffered, tileId);
    }
    
    [PunRPC]
    private void TurnEnd(int tileId)
    {
        UpdateTile(tileId);
       
        bool isWin = winCondition.CheckWinCondition(tilesTable);
        bool isTie = winCondition.CheckTie(tilesTable);
        
        if (isWin || isTie)
        {
            RoundEnded();
        }
        else
        {
            if (RoomController.Instance.LocalPlayer.IsActive)
            {
                turnController.IncrementTurn();
            }
        }
        
        PrintTable();
    }
    
    private void RoundEnded()
    {
        bool isRoundWon = RoomController.Instance.LocalPlayer.IsActive;
        
        StrikeEffect(isRoundWon);

        Timer.Start(this, "RoundEndDelay", 0.5f, ()=>
        {
            MatchController.LocalInstance.EndRound(isRoundWon);
        });
    }
    
    private void UpdateTile(int id)
    {
        tiles[id].ChangeState();
        tilesTable[id / (xSize + 1), id % (ySize + 1)] = RoomController.Instance.ActivePlayer.PlayerId;
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

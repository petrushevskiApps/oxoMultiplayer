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
    //Events
    public static UnityEvent TurnStarted = new UnityEvent();
    public static UnityEvent TurnEnded = new UnityEvent();
    public static UnityBoolEvent MatchEnded = new UnityBoolEvent();
    
    public TurnController turnController;
    public int[,] tilesTable = new int[maxRow, maxColumn];

    private List<TileState> tiles = new List<TileState>();
    private WinCondition winCondition;
    private static int maxRow = 3;
    private static int maxColumn = 3;
    
    public static BoardController LocalInstance;
    
    private void Awake()
    {
        LocalInstance = this;

        winCondition = GetComponent<WinCondition>();
        turnController = GetComponent<TurnController>();
        
        tiles = GetComponentsInChildren<TileState>().ToList();
        
        tiles.ForEach(x =>
        {
            x.TileStateChange.AddListener(CompleteTurn);
        });
        
    }


    private void Start()
    {
        SetTilesTable();
        TurnStart();
    }
    
   
    private void TurnStart()
    {
        
    }
    private void CompleteTurn(int tileId)
    {
        photonView.RPC("TurnEnd", RpcTarget.All, tileId);
    }

    [PunRPC]
    private void TurnEnd(int tileId)
    {
        SetTilesTable(tileId);
       
        bool isWin = winCondition.CheckWinCondition(tilesTable);
        bool isTie = winCondition.CheckTie(tilesTable);
        
        if (isWin || isTie)
        {
            MatchEnd();
        }
        else
        {
            TurnEnded.Invoke();
            TurnStart();
        }

        if (NetworkManager.Instance.RoomController.LocalPlayer.IsActive())
        {
            turnController.IncrementTurn();
        }
        PrintTable();
    }
    private void MatchEnd()
    {
        bool isWin = NetworkManager.Instance.RoomController.LocalPlayer.IsActive();
        
        List<WinCondition.RowColumIndex> rowColumIndex = winCondition.GetWinIndexes();
            
        rowColumIndex.ForEach(index =>
        {
            tiles[GetTileId(index.row, index.column)].EndGameTileEffect(isWin);
        });
        
        StartCoroutine(Delay(() =>
        {
            UIManager.Instance.OpenScreen<UIEndScreen>();
            MatchEnded.Invoke(isWin);
        }));
    }

    private IEnumerator Delay(Action delayedAction)
    {
        yield return new WaitForSeconds(0.5f);
        delayedAction.Invoke();
    }
   
    private void SetTilesTable(int tileId = -1)
    {
        int xSize = tilesTable.GetUpperBound(0);
        int ySize = tilesTable.GetUpperBound(1);
        if (tileId == -1)
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    tilesTable[i,j] = 0;
                }
            }
        }
        else
        {
            UpdateTile(tileId);
            tilesTable[tileId / (xSize + 1), tileId % (ySize + 1)] = NetworkManager.Instance.RoomController.ActivePlayer.PlayerId;
        }
    }

    private int GetTileId(int row, int column)
    {
        return (row * maxColumn) + column;
    }
    private void UpdateTile(int id)
    {
        tiles[id].ChangeState();
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class BoardController : MonoBehaviourPunCallbacks, IPunObservable
{
    private List<TileState> tiles = new List<TileState>();
    
    private WinCondition winCondition;
    
    public TurnController turnController;

    private static int maxRow = 3;
    private static int maxColumn = 3;
    
    public int[,] tilesTable = new int[maxRow, maxColumn];
    
    public static UnityEvent OnTurnStarted = new UnityEvent();
    public static UnityEvent OnTurnEnded = new UnityEvent();
    public static MatchEndedEvent OnMatchEnded = new MatchEndedEvent();

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static BoardController LocalInstance;
    private void Awake()
    {
        LocalInstance = this;
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.

        winCondition = GetComponent<WinCondition>();
        turnController = GetComponent<TurnController>();
        
        tiles = GetComponentsInChildren<TileState>().ToList();
        
        tiles.ForEach(x =>
        {
            x.OnStateChange.AddListener(CompleteTurn);
        });
        
        InstantiatePlayer(); 
    }
    private void InstantiatePlayer()
    {
        if (Player.LocalInstance == null)
        {
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<Player>();
        }
    }
    
    private void Start()
    {
        SetTilesTable();
        TurnStart();
    }
    
   
    private void TurnStart()
    {
        
    }
    public void CompleteTurn(int tileId)
    {
        photonView.RPC("TurnEnded", RpcTarget.All, tileId);
    }

    public void Restart()
    {
        photonView.RPC("RestartBoard", RpcTarget.All);
    }
    [PunRPC]
    private void TurnEnded(int tileId)
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
            OnTurnEnded.Invoke();
            TurnStart();
        }
        
        turnController.IncrementTurn();
        PrintTable();
    }
    private void MatchEnd()
    {
        
        bool isWin = turnController.GetActivePlayer().GetPlayerId() == Player.LocalInstance.GetPlayerId();
        
        List<WinCondition.RowColumIndex> rowColumIndex = winCondition.GetWinIndexes();
            
        rowColumIndex.ForEach(index =>
        {
            tiles[GetTileId(index.row, index.column)].EndGameTileEffect(isWin);
        });
        
        Debug.Log("Player " + Player.LocalInstance.GetPlayerId() + " Won ");
       
        
        
        StartCoroutine(Delay(() =>
        {
            UIManager.Instance.OpenScreen<UIEndScreen>();
            OnMatchEnded.Invoke(isWin);
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
            tilesTable[tileId / (xSize + 1), tileId % (ySize + 1)] = turnController.GetActivePlayer().GetPlayerId();
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
    
    public class MatchEndedEvent : UnityEvent<bool> {}


    [PunRPC]
    public void RestartBoard()
    {
        turnController.Restart();
        winCondition.Restart();
        
        tiles.ForEach(x =>
        {
            x.SetTile();
        });
        tilesTable = new int[3, 3];
        UIManager.Instance.OpenScreen<UIGameScreen>();
        TurnStart();
    }
}

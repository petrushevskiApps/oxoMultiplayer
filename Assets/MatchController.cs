using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.petrushevskiapps.Oxo;
using Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class MatchController : MonoBehaviourPunCallbacks, IPunObservable
{
    private WinCondition winCondition;
    private List<TileState> tiles = new List<TileState>();
    public TurnController turnController;
    
    public int[,] tilesTable = new int[3, 3];
    
    public static UnityEvent OnTurnStarted = new UnityEvent();
    public static UnityEvent OnTurnEnded = new UnityEvent();
    public static MatchEndedEvent OnMatchEnded = new MatchEndedEvent();
    
    
    
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static MatchController LocalInstance;
    private void Awake()
    {
//        if (photonView.IsMine)
//        {
//            
//        }
        LocalInstance = this;
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
        
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
//            Player.LocalInstance.Setup(PhotonNetwork.CurrentRoom.PlayerCount - 1);
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

    [PunRPC]
    private void TurnEnded(int tileId)
    {
        
        SetTilesTable(tileId);
       
        
        bool isWin = winCondition.CheckWinCondition(tilesTable);
        bool isTie = winCondition.CheckTie(tilesTable);
        
        if (isWin || isTie)
        {
            MatchEnd(isTie);
        }
        else
        {
            OnTurnEnded.Invoke();
            TurnStart();
        }
        
        turnController.IncrementTurn();
        PrintTable();
    }
    private void MatchEnd(bool isTie = false)
    {
        if (isTie)
        {
            Debug.Log("No Winner match is Tie");
            OnMatchEnded.Invoke(0);
        }
        else
        {
            Debug.Log("Player " + Player.LocalInstance.GetPlayerId() + " Won ");
            OnMatchEnded.Invoke(turnController.GetActivePlayer().GetPlayerId());
        }
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
    
    public class MatchEndedEvent : UnityEvent<int> {}

    
}

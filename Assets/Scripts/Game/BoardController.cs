using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Grid;
using Photon.Pun;
using UnityEngine;

public class BoardController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GridCreator gridCreator;

    public Tile[,] TilesTable { get; private set; }
    
    private void Awake()
    {
        MatchController.RoundCompletedEvent.AddListener(ResetBoard);
        MatchController.RoundEnded.AddListener(OnRoundEnded);
        
        SetupBoard();
    }

    
    private void OnDestroy()
    {
        MatchController.RoundCompletedEvent.RemoveListener(ResetBoard);
        MatchController.RoundEnded.RemoveListener(OnRoundEnded);
    }

    private void SetupBoard()
    {
        int rows = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("r");
        int columns = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("c");

        TilesTable = gridCreator.CreateGrid<Tile>(rows, columns);

        int tileId = 0;
        
        foreach (Tile tile in TilesTable)
        {
            tile.StateChange.AddListener(OnTileStateChange);
            tile.Id = tileId;
            tileId++;
        }
    }

    private void ResetBoard()
    {
        foreach (Tile tile in TilesTable)
        {
            tile.SetTile();
        }
    }

    private void OnTileStateChange(int tileId)
    {
        int playerId = NetworkManager.Instance.RoomController.ActivePlayer.GetId();
        NetworkManager.Instance.RoomController.SendRpc(photonView, RPCs.RPC_TURN_END, true, playerId,tileId);
    }
    
    [PunRPC]
    private void TurnEnded(int playerId, int tileId)
    {
        NetworkManager.Instance.RoomController.LocalRpcBufferCount++;
        
        UpdateTile(playerId, tileId);

        MatchController.LocalInstance.EndTurn(playerId, tileId, TilesTable);
        
    }

    private void UpdateTile(int playerId, int id)
    {
        TilesTable[Utilities.GetRowFromId(id, TilesTable), Utilities.GetColumnFromId(id, TilesTable)].ChangeState(playerId);
        
        Utilities.PrintTable(TilesTable);
    }

    private void OnRoundEnded(bool isRoundWon)
    {
        MatchController.LocalInstance.WinCondition.GetWinIds().ForEach(id =>
        {
            TilesTable[Utilities.GetRowFromId(id, TilesTable), Utilities.GetColumnFromId(id, TilesTable)].EndEffect(isRoundWon);
        });
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

}

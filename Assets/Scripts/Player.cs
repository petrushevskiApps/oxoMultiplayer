using System;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IComparable
{
    [SerializeField] private TileType tileType;
    [SerializeField] private int playerId = 0;

    public static Player LocalInstance;
    public bool IsActive { get; set; } = false;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalInstance = this;
            playerId = PhotonNetwork.IsMasterClient ? 1 : 2;
        }
        else playerId = PhotonNetwork.IsMasterClient ? 2 : 1;
        
        tileType = (TileType) playerId;
        
        DontDestroyOnLoad(this.gameObject);
        
        
    }

    private void OnDestroy()
    {
        PhotonNetwork.Destroy(photonView);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        BoardController.LocalInstance.turnController.AddPlayer(this);
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    public TileType GetPlayerSymbol()
    {
        return tileType;
    }


    public int CompareTo(object obj)
    {
        if (obj.GetType() != typeof(Player)) return -1;
        Player otherPlayer = (Player) obj;
        if (playerId < otherPlayer.playerId) return 1;
        else if (playerId > otherPlayer.playerId) return -1;
        else return 0;
    }
}

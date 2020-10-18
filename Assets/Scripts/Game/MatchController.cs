using Photon.Pun;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    private GameObject board;
    
    private void Awake()
    {
        if (BoardController.LocalInstance == null && PhotonNetwork.IsMasterClient)
        {
            board = PhotonNetwork.InstantiateRoomObject("Board", Vector3.zero, Quaternion.identity);
        }
    }

}

using Photon.Pun;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    private void Awake()
    {
        if (BoardController.LocalInstance == null && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("Board", Vector3.zero, Quaternion.identity);
        }
    }
}

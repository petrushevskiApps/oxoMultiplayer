using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartScene()
    {
        board.GetComponent<BoardController>().Restart();
    }
    
}

using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PhotonHelper : MonoBehaviour
    {
        [MenuItem("Photon Helper/Disconnect")]
        static void Disconnect()
        {
            Debug.Log("Disconnect photon!");
            PhotonNetwork.Disconnect();
        }
    }
}
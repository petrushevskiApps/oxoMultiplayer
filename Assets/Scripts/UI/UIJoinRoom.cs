using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinRoom : MonoBehaviour
{
    private Button createRoomBtn;
    [SerializeField] private InputField inputField;
    private void Awake()
    {
        createRoomBtn = GetComponent<Button>();
        createRoomBtn.onClick.AddListener(JoinRoom);
    }

    private void OnDestroy()
    {
        createRoomBtn.onClick.RemoveListener(JoinRoom);
    }

    private void JoinRoom()
    {
        GameManager.Instance.NetworkManager.JoinRoom(inputField.text);
    }
}

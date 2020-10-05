using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
}

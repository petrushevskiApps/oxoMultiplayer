using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

[CreateAssetMenu(order = 1, menuName = "Data/RoomScreenContext", fileName = "Context")]
public class RoomScreenContext : ScriptableObject
{
    public bool startButtonStatus = false;
    public bool readyButtonStatus = false;

    [SerializeField] private List<LabelText> labelTexts = new List<LabelText>();

    private Dictionary<RoomStatus, string> textsDict = new Dictionary<RoomStatus, string>();
    private void Awake()
    {
        labelTexts.ForEach(text => textsDict.Add(text.key, text.labelText));
    }

    public string GetRoomLabel(RoomStatus currentRoomStatus)
    {
        LabelText result = labelTexts.FirstOrDefault(text => text.key == currentRoomStatus);
        return result?.labelText ?? "";
    }
}

[Serializable]
public class LabelText
{
    public RoomStatus key;
    public string labelText;
}
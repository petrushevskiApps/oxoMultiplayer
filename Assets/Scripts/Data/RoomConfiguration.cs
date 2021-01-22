using ExitGames.Client.Photon;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfiguration", menuName = "Data/RoomConfiguration", order = 1)]
public class RoomConfiguration : ScriptableObject
{
    public bool publishUserId = false;
    public byte maxPlayers = 0;
    public int rows = 0;
    public int columns = 0;

    public Hashtable GetConfigHashtable()
    {
        Hashtable configuration = new Hashtable();
        configuration.Add("r", rows);
        configuration.Add("c", columns);
        return configuration;
    }

    public string[] GetPropertiesNames()
    {
        string[] propertiesNames = new string[2];
        propertiesNames[0] = "r";
        propertiesNames[1] = "c";
        return propertiesNames;
    }
}

using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace com.petrushevskiapps.Oxo.Properties
{
    public class RoomProperties : INetworkProperties
    {
        private readonly Hashtable properties = new Hashtable();
        
        public RoomProperties()
        {
            PhotonNetwork.CurrentRoom.PlayerTtl = 30000; // 30 sec
        }
        
        public INetworkProperties Set(string key, object value)
        {
            properties.Add(key, value);
            Debug.Log(key + ": Value: " + value);
            return this;
        }
        
        public void Update()
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            properties.Clear();
        }
        
        public T GetProperty<T>(string key)
        {
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out var result);
            if (result == null) return default;
            else return (T) result;
        }
    }
}
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace com.petrushevskiapps.Oxo.Properties
{
    public class RoomProperties : INetworkProperties
    {
        private readonly Hashtable properties = new Hashtable();
        private bool cached = false;
        
        public RoomProperties()
        {
            PhotonNetwork.CurrentRoom.PlayerTtl = 30000; // 30 sec
        }
        
        public INetworkProperties Set(string key, object value)
        {
            if (properties.ContainsKey(key))
            {
                properties[key] = value;
            }
            else properties.Add(key, value);
            
            return this;
        }
        
        public void Sync()
        {
            cached = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }

        public void Updated()
        {
            cached = false;
            properties.Clear();
        }

        public T GetProperty<T>(string key)
        {
            object result = null;
            
            if (cached)
            {
                // Use cached value while server is updated
                properties.TryGetValue(key, out result);
            }
            else
            {
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out result);
            }
            Debug.Log($"RoomProperties:: {key} = {result}");
            if (result == null) return default;
            else return (T) result;
        }
    }
}
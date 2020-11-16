using com.petrushevskiapps.Oxo.Utilities;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace com.petrushevskiapps.Oxo.Properties
{
    public class PlayerProperties : INetworkProperties
    {
        private Player player;
        private readonly Hashtable properties = new Hashtable();

        public PlayerProperties(Player player)
        {
            this.player = player;
        }
        
        public INetworkProperties Set(string key, object value)
        {
            if(!player.IsLocal) return this;

            if (properties.ContainsKey(key))
            {
                properties[key] = value;
            }
            else properties.Add(key, value);
            
            return this;
        }

        public void Sync()
        {
            if (!player.IsLocal) return;
            player.SetCustomProperties(properties);
        }

        public T GetProperty<T>(string key)
        {
            object result = null;
            
            if (properties.ContainsKey(key))
            {
                // Use cached value while server is updated
                properties.TryGetValue(key, out result);
            }
            else
            {
                player.CustomProperties.TryGetValue(key, out result);
            }
            
            Debug.Log($"Properties::Player:: {key} = {result}");
            if (result == null) return default;
            else return (T) result;
        }

        public void Updated(string key)
        {
            if (properties.ContainsKey(key))
            {
                properties.Remove(key);
            }
        }
    }
}
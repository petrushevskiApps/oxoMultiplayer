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

        private bool cached = false;
        
        public PlayerProperties(Player player)
        {
            this.player = player;

            // Set default values
            Set(Keys.PLAYER_READY_KEY, false)
                .Set(Keys.PLAYER_MATCH_ID, -1)
                .Set(Keys.PLAYER_MATCH_SCORE, 0)
                .Sync();
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
            
            cached = true;
            player.SetCustomProperties(properties);
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
                player.CustomProperties.TryGetValue(key, out result);
            }
            
            if (result == null) return default;
            else return (T) result;
        }

        public void Updated()
        {
            cached = false;
            properties.Clear();
        }
    }
}
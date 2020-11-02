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
        
        public PlayerProperties(Player player, int turnId)
        {
            this.player = player;

            Set(Keys.PLAYER_READY_KEY, false)
                .Set(Keys.PLAYER_MATCH_ID, turnId)
                .Set(Keys.PLAYER_MATCH_SCORE, 0)
                .Update();
        }
        
        public INetworkProperties Set(string key, object value)
        {
            if(!player.IsLocal) return this;
            
            properties.Add(key, value);
            return this;
        }

        public void Update()
        {
            if (player.IsLocal)
            {
                player.SetCustomProperties(properties);
            }
            properties.Clear();
        }

        public T GetProperty<T>(string key)
        {
            player.CustomProperties.TryGetValue(key, out var result);
            if (result == null) return default;
            else return (T) result;
        }
    }
}
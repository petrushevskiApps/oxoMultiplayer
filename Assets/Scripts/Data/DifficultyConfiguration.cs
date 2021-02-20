using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DifficultyConfiguration", menuName = "Data/DifficultyConfiguration", order = 1)]
    public class DifficultyConfiguration : ScriptableObject
    {
        [SerializeField] private List<Difficulty> buttonConfigurations = new List<Difficulty>();

        public int Count => buttonConfigurations.Count;
        public Difficulty GetDifficultyAt(int id)
        {
            return buttonConfigurations[id];
        }
    }

    [Serializable]
    public class Difficulty
    {
        public string description;
        public RoomConfiguration configuration;
    }
}
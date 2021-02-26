using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AiNicknames", menuName = "Data/AiNicknames", order = 1)]
    public class AiNicknames : ScriptableObject
    {
        [SerializeField] private List<string> names;
        [SerializeField] private List<string> adjectives;
        [SerializeField] private List<string> colors;
        [SerializeField] private List<string> nouns;

        private readonly List<List<string>> parts = new List<List<string>>();

        private int RandomIndex (int count) => UnityEngine.Random.Range(0, count);

        public string GenerateRandomName()
        {
            if(parts.Count == 0)
            {
                InitializePartsList();
            }
            StringBuilder name = new StringBuilder();
            name.Append(Captialize(GetRandomPart(parts[RandomIndex(parts.Count)])));
            name.Append(Captialize(GetRandomPart(parts[RandomIndex(parts.Count)])));
            
            return name.ToString();
        }
        private void InitializePartsList()
        {
            parts.Add(names);
            parts.Add(adjectives);
            parts.Add(colors);
            parts.Add(nouns);
        }

        public string GetRandomPart(List<string> part)
        {
            return part[RandomIndex(part.Count - 1)];
        }

        public string GetAdjective()
        {
            return adjectives[RandomIndex(adjectives.Count - 1)];
        }
        public string GetColor()
        {
            return colors[RandomIndex(colors.Count - 1)];
        }

        public string GetNouns()
        {
            return Captialize(nouns[RandomIndex(nouns.Count - 1)]);
        }
        private string Captialize(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }
    }
}
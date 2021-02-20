using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GuideConfiguration", menuName = "Data/GuideConfiguration", order = 1)]
    public class GuideConfiguration : ScriptableObject
    {
        [SerializeField] private List<GuidePage> guideConfigurations = new List<GuidePage>();

        public int Pages => guideConfigurations.Count;
        
        public GuidePage GetPageInfoAt(int id)
        {
            return guideConfigurations[id];
        }
    }

    [Serializable]
    public class GuidePage
    {
        public Sprite image;
        public string text;
    }
}
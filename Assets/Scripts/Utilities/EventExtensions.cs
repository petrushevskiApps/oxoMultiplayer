using UnityEngine;
using UnityEngine.Events;

namespace com.petrushevskiapps.Oxo.Utilities
{
    public class EventExtensions
    {
        
    }
    
    public class UnityBoolEvent : UnityEvent<bool>
    {
    }
    public class UnityIntegerEvent : UnityEvent<int>
    {
    }
    public class UnityStringEvent : UnityEvent<string>
         {
         }
    public class UnityTextureEvent : UnityEvent<Texture2D>
    {
    }
    
}
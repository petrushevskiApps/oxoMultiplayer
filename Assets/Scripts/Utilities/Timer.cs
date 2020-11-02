using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.petrushevskiapps.Oxo.Utilities
{
    public static class Timer
    {
        private static Dictionary<string, Coroutine> activeTimers = new Dictionary<string, Coroutine>();
        
        public static void Start(MonoBehaviour starter, string timerId, float time, Action onTimerComplete)
        {
            if(activeTimers.ContainsKey(timerId)) return;
            
            Coroutine timer = starter.StartCoroutine(DelayAction(time,timerId, onTimerComplete));
            activeTimers.Add(timerId, timer);
        }

        public static void Stop(MonoBehaviour stoper, string timerId)
        {
            stoper.StopCoroutine(activeTimers[timerId]);
            activeTimers.Remove(timerId);
        }
        
        static IEnumerator DelayAction(float time,string timerId, Action onTimerComplete)
        {
            yield return new WaitForSeconds(time);
            activeTimers.Remove(timerId);
            onTimerComplete.Invoke();
        }
    }
}
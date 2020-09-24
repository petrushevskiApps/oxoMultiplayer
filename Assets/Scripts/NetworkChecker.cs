using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.petrushevskiapps.Oxo
{
    public class NetworkChecker : MonoBehaviour
    {
        [SerializeField] private string pingUrl = "http://google.com";
        [SerializeField] private float pingSeconds = 5f;
        
        public NetworkStatusChange OnNetworkStatusChange = new NetworkStatusChange();
        public UnityEvent OnOnline = new UnityEvent();
        public UnityEvent OnOffline = new UnityEvent();
        
        public bool IsOnline => currNetworkStatus;
        
        private bool prevNetworkStatus = false;
        private bool currNetworkStatus = false;
        
        private void Start()
        {
            StartCoroutine(CheckInternetConnection());
            StartCoroutine(PeriodicCheck());
        }
        private IEnumerator PeriodicCheck()
        {
            while (true)
            {
                yield return new WaitUntil(() => currNetworkStatus != prevNetworkStatus);
                Debug.Log("Status changed: " + currNetworkStatus);
                prevNetworkStatus = currNetworkStatus;
                OnNetworkStatusChange.Invoke(currNetworkStatus);
                
                if(currNetworkStatus) OnOnline.Invoke();
                else OnOffline.Invoke();
            }
        }

        private IEnumerator CheckInternetConnection()
        {
            while (true)
            {
                WWW www = new WWW(pingUrl);
                yield return www;
                currNetworkStatus = www.error == null;
                
                yield return new WaitForSecondsRealtime(pingSeconds);
            }
            
        }
        
        public class NetworkStatusChange : UnityEvent<bool>{}
    }
}
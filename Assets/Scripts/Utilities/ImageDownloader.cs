using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace com.petrushevskiapps.Oxo.Utilities
{
    public static class ImageDownloader
    {
        public static void DownloadImage(MonoBehaviour caller, string url, Action<Texture2D> onComplete)
        {
            caller.StartCoroutine(LoadPictureRoutine(url, onComplete));
        }
        private static IEnumerator LoadPictureRoutine(string url, Action<Texture2D> onComplete)
        {
            Debug.Log($"URL:: {url}");
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            onComplete.Invoke(texture);
        }

    }
}
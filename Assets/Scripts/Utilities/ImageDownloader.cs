using System;
using System.Collections;
using UnityEngine;

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
            WWW www = new WWW(url);
            yield return www;

            if (www.error != null)
            {
                Debug.LogError(www.error);
                yield break;
            }
            
            onComplete.Invoke(www.texture);
        }
    }
}
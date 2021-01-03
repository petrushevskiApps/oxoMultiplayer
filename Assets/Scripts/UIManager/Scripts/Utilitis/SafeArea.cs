using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] private bool activateSafeArea = false;

    private RectTransform screenRect;
    protected void Awake()
    {
        screenRect = GetComponent<RectTransform> ();
    }
    protected void OnEnable()
    {
        if(activateSafeArea) ApplySafeArea();
    }
    private void ApplySafeArea ()
    {
        Rect safeArea = Screen.safeArea;
            
        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        screenRect.anchorMin = anchorMin;
        screenRect.anchorMax = anchorMax;
            
        Debug.Log("Safe area set");
    }
}

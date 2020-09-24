using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAnimatedButton : Button
{
    public UnityEvent onClickUp = new UnityEvent();

    private RectTransform rectTransform;
    private Vector3 scale = new Vector3(0.85f, 0.85f, 0.85f);

    private Coroutine scaleDownCoroutine;
    private Coroutine scaleUpCoroutine;
    
    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onClickUp.RemoveAllListeners();
        onClick.RemoveAllListeners();
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        rectTransform.localScale = Vector3.one;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        scaleDownCoroutine = StartCoroutine(Scale(scale, null));
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        CheckCoroutineStatus(ref scaleDownCoroutine);
        scaleUpCoroutine = StartCoroutine(Scale(Vector3.one, onClickUp));
    }
    
    private void CheckCoroutineStatus(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    
    private IEnumerator Scale(Vector3 newScale, UnityEvent onScaleComplete)
    {
        Vector3 lScale = rectTransform.localScale;
        
        while (Vector3.Distance(lScale, newScale) >= 0.01f)
        {
            lScale = Vector3.Lerp(lScale, newScale, Time.deltaTime * 25f);
            rectTransform.localScale = lScale;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Smooth out the values of rect scale
        rectTransform.localScale = newScale;
        
        onScaleComplete?.Invoke();
    }

}

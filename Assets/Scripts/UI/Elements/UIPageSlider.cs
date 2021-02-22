using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPageSlider : MonoBehaviour
{
    
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    
    private GridLayoutGroup layout;
    private RectTransform content;

    private float paginationSize;
    private int elementsCount = 0;
    private int currentElement = 1;
    
    private void Awake()
    {
        layout = GetComponent<GridLayoutGroup>();
        content = GetComponent<RectTransform>();
        
        leftButton.onClick.AddListener(SlidePageLeft);
        rightButton.onClick.AddListener(SlidePageRight);

        paginationSize = layout.cellSize.x + layout.spacing.x;
        elementsCount = gameObject.transform.childCount;
        
    }

    private void SlidePageRight()
    {
        if(currentElement + 1 > elementsCount) return;

        currentElement++;
        Vector2 currentSize = content.anchoredPosition;
        content.anchoredPosition = new Vector2(currentSize.x + ((-1) * paginationSize), currentSize.y);
        
    }

    private void SlidePageLeft()
    {
        if(currentElement - 1 <= 0) return;

        currentElement--;
        Vector2 currentSize =  content.anchoredPosition;
        content.anchoredPosition = new Vector2(currentSize.x + paginationSize, currentSize.y);
    }

    
}

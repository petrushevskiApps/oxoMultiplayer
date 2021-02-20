using System.Collections;
using System.Collections.Generic;
using Data;
using PetrushevskiApps.UIManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Guide : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private UIButton leftArrow;
    [SerializeField] private UIButton rightArrow;

    [SerializeField] private Image guideImage;
    [SerializeField] private TextMeshProUGUI guideText;

    [SerializeField] private GuideConfiguration guideConfig;
    
    private int currentPage;
    
    private void Awake()
    {
        leftArrow.onClick.AddListener(OnLeftArrowClicked);
        rightArrow.onClick.AddListener(OnRightArrowClicked);

        SetPage();
        ToggleArrows();
    }

    private void SetPage()
    {
        SetText();
        SetImage();
    }
    private void SetText()
    {
        guideText.text = guideConfig.GetPageInfoAt(currentPage).text;
    }

    private void SetImage()
    {
        guideImage.sprite = guideConfig.GetPageInfoAt(currentPage).image;
    }
    
    private void OnLeftArrowClicked()
    {
        if (currentPage > 0)
        {
            currentPage--;
            SetPage();
            ToggleArrows();
        }
    }
    private void OnRightArrowClicked()
    {
        if (currentPage < (guideConfig.Pages - 1))
        {
            currentPage++;
            SetPage();
            ToggleArrows();
        }
    }
    
    private void ToggleArrows()
    {
        leftArrow.interactable = currentPage > 0;

        rightArrow.interactable = currentPage < (guideConfig.Pages - 1);
    }
}

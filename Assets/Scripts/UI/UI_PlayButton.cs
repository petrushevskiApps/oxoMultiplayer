using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using Data;
using PetrushevskiApps.UIManager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class UI_PlayButton : MonoBehaviour
{
    [SerializeField] public UIButton leftArrow;
    [SerializeField] public UIButton rightArrow;
    [SerializeField] public UIButton play;
    [SerializeField] public TextMeshProUGUI description;

    [SerializeField] public DifficultyConfiguration config;
    private int configurationId;
    
    private void Awake()
    {
        leftArrow.onClick.AddListener(OnLeftArrowClicked);
        rightArrow.onClick.AddListener(OnRightArrowClicked);
        play.onClick.AddListener(OnPlayClicked);
        
        SetDescription();
        ToggleArrows();
    }

    private void SetDescription()
    {
        description.text = config.GetDifficultyAt(configurationId).description;
    }

    protected virtual void OnPlayClicked()
    {
        UIManager.Instance.OpenScreen<UILoadingScreen>();
        
        NetworkManager.Instance.SetConfiguration(config.GetDifficultyAt(configurationId).configuration);
    }

    private void OnLeftArrowClicked()
    {
        if (configurationId > 0)
        {
            configurationId--;
            SetDescription();
            ToggleArrows();
        }
    }
    private void OnRightArrowClicked()
    {
        if (configurationId < (config.Count - 1))
        {
            configurationId++;
            SetDescription();
            ToggleArrows();
        }
    }

    private void ToggleArrows()
    {
        leftArrow.interactable = configurationId > 0;

        rightArrow.interactable = configurationId < (config.Count - 1);
    }
    
}

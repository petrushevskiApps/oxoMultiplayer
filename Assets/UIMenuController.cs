using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject createGameMenu;
    [SerializeField] private GameObject nicknameMenu;
    [SerializeField] private GameObject progress;

    public static UIMenuController Instance;
    private GameObject activeMenu;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMainMenu()
    {
        if (GameManager.Instance.IsUsernameSet)
        {
            logo.SetActive(true);
            backButton.SetActive(false);
            Show(mainMenu);
        }
        else ShowPlayerMenu();
    }

    public void ShowJoinMenu()
    {
        logo.SetActive(false);
        backButton.SetActive(true);
        Show(joinMenu);
    }

    public void ShowCreateMenu()
    {
        logo.SetActive(false);
        backButton.SetActive(true);
        Show(createGameMenu);
    }
    public void ShowPlayerMenu()
    {
        logo.SetActive(false);
        backButton.SetActive(false);
        Show(nicknameMenu);
    }

    public void ShowProgress()
    {
        logo.SetActive(false);
        backButton.SetActive(true);
        Show(progress);
    }

    private void Show(GameObject toShow)
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }

        activeMenu = toShow;
        toShow.SetActive(true);
    }
}

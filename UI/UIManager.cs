using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    private bool _shopMenuOn = false;
    [SerializeField] private GameObject[] menuTabs = null;
    [SerializeField] private Button[] menuButtons = null;
    [SerializeField] private GameObject[] menuSubTabsBuy = null;
    [SerializeField] private GameObject[] menuSubTabsSell = null;
    [SerializeField] private Button[] menuSubButtonsSell = null;
    [SerializeField] private Button[] menuSubButtonsBuy = null;
    [SerializeField] private GameObject shopMenu = null;

    public bool ShopMenuOn { get => _shopMenuOn; set => _shopMenuOn = value; }

    protected override void Awake()
    {
        base.Awake();

        shopMenu.SetActive(false);
    }

    private void Update()
    {
        ShopMenu();
    }

    private void ShopMenu()
    {
            if (ShopMenuOn == false)
            {
                DisableShopMenu();
            }
            else
            {
                EnableShopMenu();
            }
    }

    private void EnableShopMenu()
    {
        ShopMenuOn = true;
        Player.Instance.PlayerInputIsDisabled = true;
        Time.timeScale = 0;
        shopMenu.SetActive(true);

        System.GC.Collect();

        HighlightButtonForSelectedTab();
    }

    private void HighlightButtonForSelectedTab()
    {
        for (int i = 0; i<menuTabs.Length; i++)
        {
            if (menuTabs[i].activeSelf)
            {
                SetButtonColorToActive(menuButtons[i]);
            }
            else
            {
                SetButtonColorToInactive(menuButtons[i]);
            }
        }
    }

    private void HighlightButtonForSelectedSubTabSell()
    {
        for (int i = 0; i < menuSubTabsSell.Length; i++)
        {
            if (menuSubTabsSell[i].activeSelf)
            {
                SetButtonColorToActive(menuSubButtonsSell[i]);
            }
            else
            {
                SetButtonColorToInactive(menuSubButtonsSell[i]);
            }
        }
    }

    private void HighlightButtonForSelectedSubTabBuy()
    {
        for (int i = 0; i < menuSubTabsBuy.Length; i++)
        {
            if (menuSubTabsBuy[i].activeSelf)
            {
                SetButtonColorToActive(menuSubButtonsBuy[i]);
            }
            else
            {
                SetButtonColorToInactive(menuSubButtonsBuy[i]);
            }
        }
    }

    private void SetButtonColorToActive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.pressedColor;

        button.colors = colors;
    }

    private void SetButtonColorToInactive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.disabledColor;

        button.colors = colors;
    }

    private void DisableShopMenu()
    {
        ShopMenuOn = false;
        Player.Instance.PlayerInputIsDisabled = false;
        Time.timeScale = 1;
        shopMenu.SetActive(false);
    }

    public void SwitchShopMenuTab(int tabNum)
    {
        for(int i = 0; i<menuTabs.Length; i++)
        {
            if (i != tabNum)
            {
                menuTabs[i].SetActive(false);
            }
            else
            {
                menuTabs[i].SetActive(true);
            }
        }

        HighlightButtonForSelectedTab();
    }

    public void SwitchSubTabSell(int tabNum)
    {
        for (int i = 0; i < menuSubTabsSell.Length; i++)
        {
            if (i != tabNum)
            {
                menuSubTabsSell[i].SetActive(false);
            }
            else
            {
                menuSubTabsSell[i].SetActive(true);
            }
        }

        HighlightButtonForSelectedSubTabSell();
    }

    public void SwitchSubTabBuy(int tabNum)
    {
        for (int i = 0; i < menuSubTabsBuy.Length; i++)
        {
            if (i != tabNum)
            {
                menuSubTabsBuy[i].SetActive(false);
            }
            else
            {
                menuSubTabsBuy[i].SetActive(true);
            }
        }

        HighlightButtonForSelectedSubTabBuy();
    }
}

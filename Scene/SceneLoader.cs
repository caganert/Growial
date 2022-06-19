using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonobehaviour<SceneLoader>
{
    private bool _pauseMenuOn = false;
    private bool _InventoryUIOn = false;
    [SerializeField] private UIInventoryBar uiInventoryBar = null;
    [SerializeField] private InventoryManagement InventoryManagement = null;
    public GameObject pauseMenuUI;
    public GameObject inventoryBarUI;
    public GameObject gameClockUI;
    public GameObject inventoryUI;

    public bool PauseMenuOn { get => _pauseMenuOn; set => _pauseMenuOn = value; }
    public bool InventoryUIOn { get => _InventoryUIOn; set => _InventoryUIOn = value; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenuOn)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryUIOn)
            {
                InventoryUIClosed();
            }
            else
            {
                InventoryUIOpened();
            }
        }
    }

    public void InventoryUIOpened()
    {
        inventoryUI.SetActive(true);
        inventoryBarUI.SetActive(false);
        Time.timeScale = 0f;
        Player.Instance.PlayerInputIsDisabled = true;
        InventoryUIOn = true;

        // Trigger garbage collector
        System.GC.Collect();

        // Destroy any currently dragged items
        uiInventoryBar.DestroyCurrentlyDraggedItems();

        // Clear currently selected items
        uiInventoryBar.ClearCurrentlySelectedItems();
    }

    public void InventoryUIClosed()
    {
        inventoryUI.SetActive(false);
        inventoryBarUI.SetActive(true);
        Time.timeScale = 1f;
        Player.Instance.PlayerInputIsDisabled = false;
        InventoryUIOn = false;

        // Destroy any currently dragged items
        InventoryManagement.DestroyCurrentlyDraggedItems();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        inventoryBarUI.SetActive(true);
        gameClockUI.SetActive(true);
        Time.timeScale = 1f;
        Player.Instance.PlayerInputIsDisabled = false;
        PauseMenuOn = false;

        // Destroy any currently dragged items
        InventoryManagement.DestroyCurrentlyDraggedItems();

    }
    public void Pause()
    {
        // Destroy any currently dragged items
        uiInventoryBar.DestroyCurrentlyDraggedItems();

        // Clear currently selected items
        uiInventoryBar.ClearCurrentlySelectedItems();

        pauseMenuUI.SetActive(true);
        inventoryBarUI.SetActive(false);
        gameClockUI.SetActive(false);
        Time.timeScale = 0f;
        Player.Instance.PlayerInputIsDisabled = true;
        PauseMenuOn = true;


        // Trigger garbage collector
        System.GC.Collect();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}



using System.Collections.Generic;
using UnityEngine;

public class Chest : SingletonMonobehaviour<Chest>, ISaveable
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    private int[] selectedChestItem; // the index of the array is the inventory list, and the value is the item code

    public List<InventoryItem>[] chestLists;

    [HideInInspector] public int[] chestListCapacityIntArray; // the index of the array is the inventory list (from the InventoryLocation enum), and the value is the capacity of that inventory list

    [SerializeField] private SO_ItemList itemList = null;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closedSprite;

    [SerializeField]
    private GameObject InventoryAndChestUI;

    private bool isOpen;

    protected override void Awake()
    {
        base.Awake();

        // Create Chest Lists
        CreateChestLists();

        // Create item details dictionary
        CreateItemDetailsDictionary();

        // Initailise selected inventory item array
        selectedChestItem = new int[(int)InventoryLocation.chest];

        for (int i = 0; i < selectedChestItem.Length; i++)
        {
            selectedChestItem[i] = -1;
        }

        // Get unique ID for gameobject and create save data object
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;

        GameObjectSave = new GameObjectSave();
    }

    private void OnDisable()
    {
        ISaveableDeregister();
    }


    private void OnEnable()
    {
        ISaveableRegister();
    }

    public void OpenChest()
    {
        if (isOpen == false)
        {
            isOpen = true;
            InventoryAndChestUI.SetActive(true);

        }
    }

    /// <summary>
    ///  Populates the itemDetailsDictionary from the scriptable object items list 
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    private void CreateChestLists()
    {
        chestLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            chestLists[i] = new List<InventoryItem>();
        }

        // initialise inventory list capacity array
        chestListCapacityIntArray = new int[(int)InventoryLocation.count];

        // initialise player inventory list capacity
        chestListCapacityIntArray[(int)InventoryLocation.chest] = Settings.chestInitialInventoryCapacity;
    }

    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        // if fromItem index and toItemIndex are within the bounds of the list, not the same, and greater than or equal to zero
        if (fromItem < chestLists[(int)inventoryLocation].Count && toItem < chestLists[(int)inventoryLocation].Count
             && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = chestLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = chestLists[(int)inventoryLocation][toItem];

            chestLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            chestLists[(int)inventoryLocation][fromItem] = toInventoryItem;

            //  Send event that inventory has been updated
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, chestLists[(int)inventoryLocation]);
        }
    }

    /// <summary>
    /// Clear the selected inventory item for inventoryLocation
    /// </summary>
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedChestItem[(int)inventoryLocation] = -1;
    }

    /// <summary>
    /// Returns the itemDetails (from the SO_ItemList) for the itemCode, or null if the item code doesn't exist
    /// </summary>

    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the itemDetails (from the SO_ItemList) for the currently selected item in the inventoryLocation , or null if an item isn't selected
    /// </summary>
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation)
    {
        int itemCode = GetSelectedInventoryItem(inventoryLocation);

        if (itemCode == -1)
        {
            return null;
        }
        else
        {
            return GetItemDetails(itemCode);
        }
    }


    /// <summary>
    /// Get the selected item for inventoryLocation - returns itemCode or -1 if nothing is selected
    /// </summary>
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        return selectedChestItem[(int)inventoryLocation];
    }



    /// <summary>
    /// Get the item type description for an item type - returns the item type description as a string for a given ItemType
    /// </summary>

    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool;
                break;

            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool;
                break;

            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool;
                break;

            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool;
                break;

            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool;
                break;

            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool;
                break;

            default:
                itemTypeDescription = itemType.ToString();
                break;
        }

        return itemTypeDescription;
    }




    /// <summary>
    /// Add item to the end of the inventory


    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // Nothing required her since the inventory manager is on a persistent scene;
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // Nothing required here since the inventory manager is on a persistent scene;
    }

    public GameObjectSave ISaveableSave()
    {
        // Create new scene save
        SceneSave sceneSave = new SceneSave();

        // Remove any existing scene save for persistent scene for this gameobject
        GameObjectSave.sceneData.Remove(Settings.PersistentScene);

        // Add inventory lists array to persistent scene save
        sceneSave.listInvItemArray = chestLists;

        // Add  inventory list capacity array to persistent scene save
        sceneSave.intArrayDictionary = new Dictionary<string, int[]>();
        sceneSave.intArrayDictionary.Add("chestListCapacityArray", chestListCapacityIntArray);

        // Add scene save for gameobject
        GameObjectSave.sceneData.Add(Settings.PersistentScene, sceneSave);

        return GameObjectSave;
    }


    public void ISaveableLoad(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // Need to find inventory lists - start by trying to locate saveScene for game object
            if (gameObjectSave.sceneData.TryGetValue(Settings.PersistentScene, out SceneSave sceneSave))
            {
                // list inv items array exists for persistent scene
                if (sceneSave.listInvItemArray != null)
                {
                    chestLists = sceneSave.listInvItemArray;

                    //  Send events that inventory has been updated
                    for (int i = 0; i < (int)InventoryLocation.count; i++)
                    {
                        EventHandler.CallInventoryUpdatedEvent((InventoryLocation)i, chestLists[i]);
                    }
                }
                // int array dictionary exists for scene
                if (sceneSave.intArrayDictionary != null && sceneSave.intArrayDictionary.TryGetValue("chestListCapacityArray", out int[] chestCapacityArray))
                {
                    chestListCapacityIntArray = chestCapacityArray;
                }
            }

        }
    }
}

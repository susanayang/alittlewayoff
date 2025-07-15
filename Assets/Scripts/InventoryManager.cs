using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int maxItemStack = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryGroup;
    public GameObject inventoryButton;

    private InventoryItem selectedItem;
    private InventorySlot selectedSlot;

    private void Awake()
    {
        Instance = this;
        // To get selected item in other scripts:
        // Item item = InventoryManager.instance.GetSelectedItem(false);    
    }

    private void Start()
    {
        // ChangeSelectedSlot(0);
        // Debug.Log($"{selectedSlot}");
    }

    private void Update()
    {
        // choose slot 1-7, probably don't need
        // if (Input.inputString != null)
        // {
        //     bool isNumber = int.TryParse(Input.inputString, out int number);
        //     if (isNumber && number > 0 && number < 8)
        //     {
        //         ChangeSelectedSlot(number - 1);
        //     }
        // }
    }

    //different implementation may be needed for mobile tap
    // void ChangeSelectedSlot(int newSelectedSlot)
    // {
    //     if (selectedSlot >= 0) {
    //         inventorySlots[selectedSlot].Deselect();
    //     }

    //     inventorySlots[newSelectedSlot].Select();
    //     selectedSlot = newSelectedSlot;
    // }

    public void SelectItem(InventoryItem item)
    {
        selectedItem = item;
        selectedSlot = item.transform.parent.GetComponent<InventorySlot>();

        Debug.Log($"Selected {selectedItem} in slot {selectedSlot.name}");
        CloseInventory();
    }

    public bool HaveSelectedItem()
    {
        return selectedItem != null;
    }

    public Item GetSelectedItem(bool use)
    {
        // get/use item in selectedSlot first
        InventoryItem itemInSlot = selectedSlot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    FindNextItemSlot();
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }

        return null;
    }

    public void FindNextItemSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == selectedItem.item && slot != selectedSlot)
            {
                selectedItem = itemInSlot;
                selectedSlot = slot;
                return;
            }
        }
    }

    private void OnEnable()
    {
        PlantGrowth.OnItemPicked += OnItemPickedHandler;
    }

    private void OnDisable()
    {
        PlantGrowth.OnItemPicked -= OnItemPickedHandler;
    }

    private void OnItemPickedHandler(Item item)
    {
        bool success = AddItem(item);
        if (!success)
        {
            Debug.LogWarning("Failed to add item to inventory!");
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxItemStack && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public void CloseInventory()
    {
        inventoryGroup.SetActive(false);
        inventoryButton.SetActive(true);

    }
}

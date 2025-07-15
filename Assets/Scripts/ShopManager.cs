using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public ShopSlot[] shopSlots;
    public GameObject shopItemPrefab;
    public GameObject shopGroup;
    public GameObject shopButton;
    public Item[] availableItems;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (ShopSlot slot in shopSlots)
        {
            int randomIndex = Random.Range(0, availableItems.Length);
            Item randomItem = availableItems[randomIndex];
            GameObject newItem = Instantiate(shopItemPrefab, slot.transform);
            ShopItem shopItem = newItem.GetComponent<ShopItem>();
            int randomCount = Random.Range(5, 11);
            shopItem.count = randomCount;
            shopItem.InitializeItem(randomItem);
        }
    }
}

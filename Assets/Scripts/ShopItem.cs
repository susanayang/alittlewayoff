using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;
    // public GameObject mainInventoryGroup;
    public TextMeshProUGUI priceText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    public Button buyButton;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;

        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
        priceText.text = (item.buyingPrice * count).ToString();
    }
}


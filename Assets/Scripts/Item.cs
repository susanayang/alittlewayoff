using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/item")]
public class Item : ScriptableObject
{
    // public TileBase tile;
    [Header("Only Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
    public int buyingPrice;

    public enum ItemType
    {
        BuildingBlock,
        Tool
    }

    public enum ActionType
    {
        Dig,
        Mine
    }
}

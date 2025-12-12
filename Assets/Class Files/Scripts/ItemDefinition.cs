using UnityEngine;

public enum ItemType
{
    Generic,
    FishingRod,
    Fish,
    Bait,
    Tool,
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Header("Basic Info")]
    public string id;              // e.g. "fishing_rod_basic"
    public string displayName;     // e.g. "Wooden Fishing Rod"
    [TextArea] public string description;

    [Header("Visuals")]
    public Sprite icon;            // for UI inventory
    public GameObject prefab;      // 3D model to spawn/equip in XR

    [Header("Item Settings")]
    public ItemType itemType = ItemType.Generic;
    public bool stackable = false;
    public int maxStack = 1;
}


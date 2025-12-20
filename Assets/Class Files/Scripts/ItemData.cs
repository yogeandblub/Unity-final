using UnityEngine;

public enum ItemType
{
    Tool,
    Fish,
    Bait,
    Other
}

[CreateAssetMenu(menuName = "FishingGame/Item")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string displayName;

    [TextArea] public string description;
    public ItemType type = ItemType.Other;

    [Header("UI")]
    public Sprite icon;

    [Header("World")]
    public GameObject worldPrefab;          // prefab to spawn into the world
    public bool spawnWithPhysics = true;    // if true, keep Rigidbody physics on spawn

    [Header("Equip")]
    public bool equipToHand = false;        // if true, tap equips instead of spawning (tools)
    public GameObject equippedPrefab;       // optional different prefab for hand equip
}

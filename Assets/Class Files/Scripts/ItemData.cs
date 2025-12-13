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
    public string id;                    // optional, for identification
    public string displayName;
    [TextArea] public string description;
    public ItemType type;

    public Sprite icon;                  // thumbnail for UI
    public GameObject worldPrefab;       // prefab to spawn in 3D world
}

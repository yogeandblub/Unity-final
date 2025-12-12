using UnityEngine;

public class PlayerInventoryStarter : MonoBehaviour
{
    public Inventory inventory;
    public ItemDefinition startingFishingRod;

    private void Start()
    {
        if (inventory == null)
            inventory = GetComponent<Inventory>();

        if (inventory != null && startingFishingRod != null)
        {
            inventory.AddItem(startingFishingRod, 1);
        }
    }
}

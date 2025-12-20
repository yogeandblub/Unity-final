using UnityEngine;

public class InventoryStarter : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ItemData[] testItems;
    [SerializeField] private int amountEach = 1;

    private void Start()
    {
        if (!inventory)
        {
            Debug.LogError("[InventoryStarter] Missing Inventory reference.");
            return;
        }

        if (testItems == null || testItems.Length == 0)
        {
            Debug.Log("[InventoryStarter] No test items assigned.");
            return;
        }

        foreach (var item in testItems)
        {
            if (item) inventory.AddItem(item, amountEach);
        }

        Debug.Log($"[InventoryStarter] Added {testItems.Length} test items.");
    }
}

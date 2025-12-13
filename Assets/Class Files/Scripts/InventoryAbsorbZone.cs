using UnityEngine;

public class InventoryAbsorbZone : MonoBehaviour
{
    public Inventory inventory;
    public bool destroyOnAbsorb = true;

    private void OnTriggerEnter(Collider other)
    {
        var itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld == null || itemWorld.data == null || inventory == null)
            return;

        Debug.Log($"[InventoryAbsorb] {itemWorld.data.displayName} x{itemWorld.amount}");

        inventory.AddItem(itemWorld.data, itemWorld.amount);

        if (destroyOnAbsorb)
            Destroy(itemWorld.gameObject);
    }
}

using UnityEngine;

public class InventoryAbsorbZone : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private float destroyDelay = 0f;

    private void OnTriggerEnter(Collider other)
    {
        var itemWorld = other.GetComponentInParent<ItemWorld>();
        if (!itemWorld || itemWorld.Data == null) return;

        inventory.AddItem(itemWorld.Data, itemWorld.Amount);

        if (destroyDelay <= 0f) Destroy(itemWorld.gameObject);
        else Destroy(itemWorld.gameObject, destroyDelay);

        Debug.Log($"[AbsorbZone] Absorbed: {itemWorld.Data.displayName} x{itemWorld.Amount}");
    }
}

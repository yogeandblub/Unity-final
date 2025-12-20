using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [Header("Attach Points")]
    [SerializeField] private Transform rightHandAttach; // assign a hand anchor / attach point
    [SerializeField] private Transform leftHandAttach;

    private GameObject _equippedGO;

    public void Unequip()
    {
        if (_equippedGO) Destroy(_equippedGO);
        _equippedGO = null;
    }

    public void Equip(ItemData data, bool rightHand = true)
    {
        if (data == null) return;

        Transform attach = rightHand ? rightHandAttach : leftHandAttach;
        if (!attach)
        {
            Debug.LogError("[EquipManager] Missing hand attach transform.");
            return;
        }

        Unequip();

        var prefab = data.equippedPrefab ? data.equippedPrefab : data.worldPrefab;
        if (!prefab)
        {
            Debug.LogError($"[EquipManager] No prefab to equip for item: {data.displayName}");
            return;
        }

        _equippedGO = Instantiate(prefab, attach);
        _equippedGO.transform.localPosition = Vector3.zero;
        _equippedGO.transform.localRotation = Quaternion.identity;

        // Make sure it doesn't fight physics in-hand
        var rb = _equippedGO.GetComponentInChildren<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Debug.Log($"[EquipManager] Equipped: {data.displayName}");
    }
}

using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemData data;
    public int amount = 1;

    // Optional: helper to initialize in code
    public void Init(ItemData itemData, int amt = 1)
    {
        data = itemData;
        amount = amt;
    }
}

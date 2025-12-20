using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemData Data { get; private set; }
    public int Amount { get; private set; } = 1;

    public void Init(ItemData data, int amount = 1)
    {
        Data = data;
        Amount = Mathf.Max(1, amount);
    }
}

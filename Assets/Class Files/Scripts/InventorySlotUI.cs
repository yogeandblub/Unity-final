using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Visuals")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text amountText;

    [Header("Hold")]
    [SerializeField] private float holdThreshold = 0.5f;

    private InventoryUI _ui;
    private int _index;

    private InventoryItem _item;
    private bool _pressed;
    private float _heldTime;
    private bool _holdFired;

    public void Bind(InventoryUI ui, int index)
    {
        _ui = ui;
        _index = index;
    }

    public void SetItem(InventoryItem item)
    {
        _item = item;

        bool hasItem = _item != null && _item.data != null && _item.amount > 0;

        if (iconImage)
        {
            iconImage.enabled = hasItem && _item.data.icon != null;
            iconImage.sprite = hasItem ? _item.data.icon : null;
        }

        if (amountText)
        {
            if (!hasItem) amountText.text = "";
            else amountText.text = _item.amount > 1 ? _item.amount.ToString() : "";
        }
    }

    private void Update()
    {
        if (!_pressed) return;

        _heldTime += Time.deltaTime;

        if (!_holdFired && _heldTime >= holdThreshold)
        {
            _holdFired = true;
            Debug.Log($"[SlotUI] Hold index={_index}");
            _ui?.OnSlotHeld(_index);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_ui == null) return;

        _pressed = true;
        _heldTime = 0f;
        _holdFired = false;

        Debug.Log($"[SlotUI] Down index={_index}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_ui == null) return;

        Debug.Log($"[SlotUI] Up index={_index} holdFired={_holdFired}");

        if (_pressed && !_holdFired)
        {
            _ui.OnSlotTapped(_index);
        }

        _pressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pressed = false;
    }
}

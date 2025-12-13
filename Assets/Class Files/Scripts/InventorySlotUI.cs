using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Visuals")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("Details Popup")]
    public GameObject detailsPanel;              // e.g. a small panel
    public TextMeshProUGUI detailsNameText;
    public TextMeshProUGUI detailsDescriptionText;
    public float holdThreshold = 0.5f;

    private InventoryItem _item;
    private InventoryUI _inventoryUI;

    private bool _isHeld;
    private float _holdTime;

    public void Setup(InventoryItem item, InventoryUI inventoryUI)
    {
        _item = item;
        _inventoryUI = inventoryUI;

        if (item != null && item.data != null)
        {
            iconImage.sprite = item.data.icon;
            iconImage.enabled = true;
            amountText.text = item.amount > 1 ? item.amount.ToString() : "";
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";
        }

        HideDetails();
    }

    public void OnClickSpawn()
    {
        if (_item == null || _item.data == null || _inventoryUI == null)
            return;

        _inventoryUI.SpawnItemFromSlot(_item);
    }

    // --- Pointer interfaces for tap vs hold ---

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHeld = true;
        _holdTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isHeld) return;
        _isHeld = false;

        // Short tap = spawn
        if (_holdTime < holdThreshold)
            OnClickSpawn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHeld = false;
        HideDetails();
    }

    private void Update()
    {
        if (!_isHeld || _item == null || _item.data == null)
            return;

        _holdTime += Time.deltaTime;
        if (_holdTime >= holdThreshold)
            ShowDetails();
    }

    private void ShowDetails()
    {
        if (detailsPanel == null) return;

        detailsPanel.SetActive(true);
        if (detailsNameText != null)
            detailsNameText.text = _item.data.displayName;
        if (detailsDescriptionText != null)
            detailsDescriptionText.text = _item.data.description;
    }

    private void HideDetails()
    {
        if (detailsPanel != null)
            detailsPanel.SetActive(false);
    }
}

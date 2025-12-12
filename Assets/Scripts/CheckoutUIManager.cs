using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutUIManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject inventoryPanel;

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }
}

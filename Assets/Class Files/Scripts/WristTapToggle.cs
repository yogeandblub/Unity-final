using UnityEngine;

public class WristTapToggle : MonoBehaviour
{
    public WristMenu wristMenu;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[WristTap] Trigger enter with {other.name}");

        if (wristMenu != null)
        {
            wristMenu.ToggleMenu();
        }
    }
}

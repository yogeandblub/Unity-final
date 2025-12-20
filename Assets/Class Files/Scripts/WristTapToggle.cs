using UnityEngine;

public class WristTapToggle : MonoBehaviour
{
    public WristMenu wristMenu;
    public float cooldown = 0.4f;

    private float _lastToggleTime;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - _lastToggleTime < cooldown) return;
        if (wristMenu == null) return;

        _lastToggleTime = Time.time;
        Debug.Log($"[WristTap] Trigger enter with {other.name}");
        wristMenu.Toggle();
    }
}

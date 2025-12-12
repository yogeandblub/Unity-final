using UnityEngine;

public class WristMenuPinchToggle : MonoBehaviour
{
    [Header("References")]
    public WristMenu wristMenu;   // reference to script on WristCanvas
    public OVRHand rightHand;     // OVRHand (right)

    [Header("Settings")]
    [Range(0f, 1f)]
    public float pinchThreshold = 0.75f;

    private bool _wasPinching = false;

    private void Update()
    {
        if (wristMenu == null || rightHand == null) return;

        float pinchStrength = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength >= pinchThreshold;

        if (isPinching && !_wasPinching)
        {
            wristMenu.ToggleMenu();
        }

        _wasPinching = isPinching;
    }
}

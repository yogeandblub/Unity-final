using UnityEngine;

public class WristTapToggle : MonoBehaviour
{
    [Header("References")]
    public WristMenu wristMenu;       // The script we wrote earlier
    public string fingerTag = "RightFinger";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(fingerTag) && wristMenu != null)
        {
            wristMenu.ToggleMenu();
        }
    }
}

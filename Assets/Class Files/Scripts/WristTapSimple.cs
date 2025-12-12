using UnityEngine;

public class WristTapSimple : MonoBehaviour
{
    [Header("Bones")]
    public Transform leftWrist;      // b_l_wrist
    public Transform rightIndexTip;  // last index bone, e.g. b_r_index3

    [Header("UI")]
    public WristMenu wristMenu;      // your existing WristMenu on WristCanvas

    [Header("Settings")]
    public float tapDistance = 0.03f;      // 3 cm
    public float minTimeBetweenTaps = 0.4f;

    private float _lastTapTime;

    void Update()
    {
        if (!leftWrist || !rightIndexTip || wristMenu == null)
            return;

        float dist = Vector3.Distance(leftWrist.position, rightIndexTip.position);

        if (dist < tapDistance && Time.time - _lastTapTime > minTimeBetweenTaps)
        {
            _lastTapTime = Time.time;
            wristMenu.ToggleMenu();
        }
    }
}

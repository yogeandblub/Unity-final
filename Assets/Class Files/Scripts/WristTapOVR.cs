using UnityEngine;

public class WristTapOVR : MonoBehaviour
{
    [Header("OVR Hands")]
    public OVRHand leftHand;
    public OVRHand rightHand;

    [Header("UI")]
    public WristMenu wristMenu;

    [Header("Gesture Settings")]
    public float tapDistance = 0.03f;       // 3 cm
    public float minTimeBetweenTaps = 0.4f; // debounce

    private Transform _leftWrist;
    private Transform _rightIndexTip;
    private float _lastTapTime;

    private void Start()
    {
        // Get skeletons
        var leftSkeleton  = leftHand.GetComponent<OVRSkeleton>();
        var rightSkeleton = rightHand.GetComponent<OVRSkeleton>();

        // Find left wrist bone
        foreach (var bone in leftSkeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_WristRoot)
            {
                _leftWrist = bone.Transform;
                break;
            }
        }

        // Find right index tip bone
        foreach (var bone in rightSkeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                _rightIndexTip = bone.Transform;
                break;
            }
        }
    }

    private void Update()
    {
        if (_leftWrist == null || _rightIndexTip == null || wristMenu == null)
            return;

        float dist = Vector3.Distance(_leftWrist.position, _rightIndexTip.position);

        if (dist < tapDistance && Time.time - _lastTapTime > minTimeBetweenTaps)
        {
            _lastTapTime = Time.time;
            wristMenu.ToggleMenu();
        }
    }
}

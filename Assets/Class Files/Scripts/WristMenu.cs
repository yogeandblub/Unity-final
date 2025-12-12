using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WristMenu : MonoBehaviour
{
    [Header("References")]
    public Transform head;    // CenterEyeAnchor
    public Transform hand;    // LeftHandAnchor

    [Header("Offsets")]
    public float distanceFromHand = 0.15f;
    public float heightOffset = 0.05f;
    public float smoothFollow = 10f;

    [Header("State")]
    public bool visible = true;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = visible;   // set once when we start
    }

    private void LateUpdate()
    {
        if (head == null || hand == null) return;
        if (!visible) return;        // only move when visible (optional)

        // Position slightly above & in front of the hand
        Vector3 handPos = hand.position;
        Vector3 toHead = (head.position - handPos).normalized;

        Vector3 targetPos = handPos
                            + Vector3.up * heightOffset
                            + toHead * distanceFromHand;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * smoothFollow
        );

        // Face the head
        Vector3 lookPos = head.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        transform.Rotate(0, 180f, 0);
    }

    public void ToggleMenu()
    {
        visible = !visible;
        _canvas.enabled = visible;
        Debug.Log($"[WristMenu] Canvas enabled = {visible}");
    }

    public void SetVisible(bool value)
    {
        visible = value;
        _canvas.enabled = visible;
        Debug.Log($"[WristMenu] SetVisible({value})");
    }
}

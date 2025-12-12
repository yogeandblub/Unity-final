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
    public bool visible = false;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = visible;
    }

    private void LateUpdate()
    {
        if (!visible || head == null || hand == null) return;

        // Position slightly above the wrist and towards the head
        Vector3 handPos = hand.position;
        Vector3 toHead = (head.position - handPos).normalized;

        Vector3 targetPos = handPos
                            + Vector3.up * heightOffset
                            + toHead * distanceFromHand;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothFollow);

        // Rotate UI to face the head
        Vector3 lookPos = head.position;
        lookPos.y = transform.position.y; // keep UI level
        transform.LookAt(lookPos);
        transform.Rotate(0, 180f, 0); // flip so front faces you
    }

    public void ToggleMenu()
    {
        visible = !visible;
        if (_canvas != null)
            _canvas.enabled = visible;

            // TEMP: test toggle with keyboard in Play Mode
    if (Input.GetKeyDown(KeyCode.T))
    {
        ToggleMenu();
    }
    }
}

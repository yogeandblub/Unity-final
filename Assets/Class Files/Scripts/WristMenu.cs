using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WristMenu : MonoBehaviour
{
    [Header("References")]
    public Transform head;         // CenterEyeAnchor
    public Transform wrist;        // LeftHandAnchor (wrist-ish)
    private Vector3 _originalScale; // Remember original scale of canvas

    [Header("Head-locked target")]
    public float headDistance = 0.6f;      // how far in front of face
    public float headHeightOffset = -0.05f; // a little below eye level
    public float moveSpeed = 5f;           // speed of fly-from-wrist animation

    private Canvas _canvas;

    private enum State { Hidden, MovingToHead, Shown }
    private State _state = State.Hidden;

    private Vector3 _targetPos;

    [Header("Hologram Motion")]
    public float bobAmplitude = 0.02f;  // 2 cm
    public float bobFrequency = 1.5f;   // speed of bob
    private float _bobTime;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;


    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;          // start hidden

        _originalScale = transform.localScale; // remember tiny world-space scale

        if (audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (head == null || wrist == null) return;
        if (_state == State.Hidden) return;

        // target in front of head (head-locked)
        Vector3 flatForward = head.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        _targetPos = head.position
                    + flatForward * headDistance
                    + Vector3.up * headHeightOffset;

        if (_state == State.MovingToHead)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                _targetPos,
                Time.deltaTime * moveSpeed
            );

            // smooth scale back up to original
            transform.localScale = Vector3.Lerp(
            transform.localScale,
            _originalScale,
            Time.deltaTime * 6f
             );

            if (Vector3.Distance(transform.position, _targetPos) < 0.01f)
            {
                _state = State.Shown;
            }
        }
        else if (_state == State.Shown)
        {
            // stick in front of face
            transform.position = _targetPos;

             // hologram bob
            _bobTime += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(_bobTime) * bobAmplitude;
            transform.position += Vector3.up * bobOffset;
        }

        // always face the head
        Vector3 lookPos = head.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        transform.Rotate(0, 180f, 0);
    }

    public void ToggleMenu()
    {
        if (_state == State.Hidden)
        {
            // spawn near wrist, pointing out from wrist
            Vector3 wristForward = wrist.forward;
            wristForward.y = 0f;
            wristForward.Normalize();

            transform.position = wrist.position
                               + wristForward * 0.10f   // 10cm out
                               + Vector3.up * 0.03f;    // a bit above wrist

            // start slightly smaller than original
            transform.localScale = _originalScale * 0.8f;
            _canvas.enabled = true;
            _state = State.MovingToHead;
            _bobTime = 0f;
            Debug.Log("[WristMenu] Open - moving to head");

            // 🔊 play open sound
        if (audioSource != null && openClip != null)
            audioSource.PlayOneShot(openClip);

        Debug.Log("[WristMenu] Open - moving to head");
    
        }
        else
        {
            _canvas.enabled = false;
            _state = State.Hidden;
            Debug.Log("[WristMenu] Closed");

            // 🔊 play close sound
            if (audioSource != null && closeClip != null)
            audioSource.PlayOneShot(closeClip);
        }
    }
}

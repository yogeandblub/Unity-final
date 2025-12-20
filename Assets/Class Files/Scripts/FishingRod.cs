using System.Collections;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class FishingRod : MonoBehaviour
{
    [Header("Grab")]
    [SerializeField] private HandGrabInteractable grab;

    [Header("Refs")]
    [SerializeField] private Transform tip;
    [SerializeField] private LineRenderer line;

    [Header("Prefabs")]
    [SerializeField] private Bobber bobberPrefab;
    [SerializeField] private GameObject[] fishPrefabs;

    [Header("Cast Motion (forward flick)")]
    [SerializeField] private float castFlickSpeed = 2.2f;
    [SerializeField] private float castForwardDotMin = 0.55f;
    [SerializeField] private float castCooldown = 0.8f;

    [Header("Bobber Launch")]
    [SerializeField] private float castSpeed = 12f;
    [SerializeField] private float castUpward = 2.5f;

    [Header("Bite Timing")]
    [SerializeField] private float biteMinSeconds = 2f;
    [SerializeField] private float biteMaxSeconds = 6f;

    [Header("Hook Set (upward flick)")]
    [SerializeField] private float hookFlickSpeed = 1.8f;
    [SerializeField] private float hookUpDotMin = 0.65f; // 1 = straight up
    [SerializeField] private float hookCooldown = 0.5f;

    private Bobber currentBobber;
    private Coroutine biteRoutine;
    private bool biteReady;

    private Vector3 lastTipPos;
    private float castCooldownUntil;
    private float hookCooldownUntil;

    private bool IsHeld => grab != null && grab.State == InteractableState.Select;

    void Start()
    {
        if (tip) lastTipPos = tip.position;
        if (line) line.useWorldSpace = false;
    }

    void Update()
    {
        UpdateLine();

        if (!tip) return;

        // Always keep lastTipPos updated so velocity doesn’t spike when you grab later
        Vector3 tipVel = (tip.position - lastTipPos) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastTipPos = tip.position;

        if (!IsHeld) return;

        // 1) Forward flick to CAST (only if no bobber out)
        if (currentBobber == null && Time.time >= castCooldownUntil)
        {
            float speed = tipVel.magnitude;
            float forwardDot = Vector3.Dot(tipVel.normalized, tip.forward);

            if (speed > castFlickSpeed && forwardDot > castForwardDotMin)
            {
                Cast();
                castCooldownUntil = Time.time + castCooldown;
            }
        }

        // 2) Upward flick to HOOK SET (only if bite is ready)
        if (biteReady && currentBobber != null && currentBobber.InWater && Time.time >= hookCooldownUntil)
        {
            float speed = tipVel.magnitude;
            float upDot = Vector3.Dot(tipVel.normalized, Vector3.up);

            if (speed > hookFlickSpeed && upDot > hookUpDotMin)
            {
                HookSetCatch();
                hookCooldownUntil = Time.time + hookCooldown;
            }
        }
    }

    void Cast()
    {
        currentBobber = Instantiate(bobberPrefab, tip.position, Quaternion.identity);

        Vector3 velocity = tip.forward * castSpeed + Vector3.up * castUpward;
        currentBobber.Launch(velocity);

        biteReady = false;
        if (biteRoutine != null) StopCoroutine(biteRoutine);
        biteRoutine = StartCoroutine(WaitForBite());
    }

    IEnumerator WaitForBite()
    {
        // Wait until bobber actually hits water
        while (currentBobber != null && !currentBobber.InWater)
            yield return null;

        if (currentBobber == null) yield break;

        // Random bite time
        float t = Random.Range(biteMinSeconds, biteMaxSeconds);
        yield return new WaitForSeconds(t);

        // Bite!
        biteReady = true;

        // tiny visual cue: bobber dips
        currentBobber.transform.position += Vector3.down * 0.06f;
    }

    void HookSetCatch()
    {
        biteReady = false;

        if (currentBobber == null) return;
        if (fishPrefabs == null || fishPrefabs.Length == 0) return;

        var fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        Instantiate(fishPrefab, currentBobber.transform.position, Quaternion.identity);

        Destroy(currentBobber.gameObject);
        currentBobber = null;

        if (biteRoutine != null) StopCoroutine(biteRoutine);
        biteRoutine = null;
    }

    void UpdateLine()
    {
        if (!line || !tip) return;

        line.positionCount = 2;
        line.SetPosition(0, tip.position);
        line.SetPosition(1, currentBobber ? currentBobber.transform.position : tip.position);
    }
}

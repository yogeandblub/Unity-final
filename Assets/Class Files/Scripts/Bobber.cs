using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bobber : MonoBehaviour
{
    public bool InWater { get; private set; }
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
    }

    public void Launch(Vector3 velocity)
    {
        InWater = false;
        rb.isKinematic = false;
        rb.velocity = velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Water")) return;

        InWater = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}

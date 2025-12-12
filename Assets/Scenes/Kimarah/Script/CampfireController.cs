using UnityEngine;
using System.Collections; // Not strictly needed for this script, but good practice

public class CampfireController : MonoBehaviour
{
    // 1. Reference to the Particle System - Set this in the Inspector!
    [SerializeField]
    private ParticleSystem fireParticleSystem;

    // 2. Settings for controlling the fire size - Tune these values in the Inspector!
    [SerializeField]
    private float sizeIncreasePerLog = 0.3f; // Size increase each time
    
    [SerializeField]
    private float baseFireSize = 0.5f; // The starting size when the first log is added
    
    [SerializeField]
    private float maxFireSize = 3.0f; // The maximum size the fire can reach

    // A flag to track if the fire has been started
    private bool isFireLit = false;

    void Start()
    {
        if (fireParticleSystem == null)
        {
            Debug.LogError("Fire Particle System reference is not set on CampfireController!", this);
            return;
        }

        // Set initial fire size to 0 if we want it invisible at start
        var mainModule = fireParticleSystem.main;
        // Sets the particle size range to 0 to make it invisible initially
        mainModule.startSize = new ParticleSystem.MinMaxCurve(0.0f, 0.0f); 
        
        // Ensure the particle system is playing, even if particles are size 0.
        fireParticleSystem.Play(); 
    }

    // Public method called by the trigger detection when a log is added
    public void AddLog()
    {
        var mainModule = fireParticleSystem.main;
        
        float currentMin = mainModule.startSize.constantMin;
        float currentMax = mainModule.startSize.constantMax;

        if (!isFireLit)
        {
            // First log: Set the fire to its base, visible size
            currentMin = baseFireSize;
            currentMax = baseFireSize + sizeIncreasePerLog;
            isFireLit = true;
        }
        else
        {
            // Subsequent logs: Increase the size, but cap it at maxFireSize
            currentMin = Mathf.Min(currentMin + sizeIncreasePerLog, maxFireSize - sizeIncreasePerLog);
            currentMax = Mathf.Min(currentMax + sizeIncreasePerLog, maxFireSize);
        }

        // Apply the new size range back to the particle system
        mainModule.startSize = new ParticleSystem.MinMaxCurve(currentMin, currentMax);
        Debug.Log($"Fire size increased to range: {currentMin:F2} - {currentMax:F2}");
    }
    
    // This function is automatically called when a collider enters the trigger collider on this object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is a Log by checking its tag
        if (other.gameObject.CompareTag("Log"))
        {
            // Call the method to increase the fire size
            AddLog();

            // Destroy the log object to simulate it being consumed by the fire.
            // This prevents the player from fueling the fire indefinitely with the same log.
            Destroy(other.gameObject); 
        }
    }
}
using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class UIFlicker : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public float flickerSpeed = 1f; // Speed of the fade
    public float minAlpha = 0.5f;   // Minimum transparency
    public float maxAlpha = 1.0f;   // Maximum transparency (fully visible)

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("UIFlicker requires a TextMeshProUGUI component.");
            enabled = false;
        }
    }

    void Update()
    {
        // Use a sine wave to smoothly transition the alpha value
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * flickerSpeed) + 1f) / 2f);
        
        // Apply the new color with the calculated alpha
        Color color = textComponent.color;
        color.a = alpha;
        textComponent.color = color;
    }
}

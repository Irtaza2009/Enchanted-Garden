using UnityEngine;
using UnityEngine.Rendering.Universal;
public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight; // Reference to the Global Light 2D
    public Color dayColor = new Color(1f, 1f, 1f, 1f); // Bright light for day
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f, 1f); // Dim light for night
    public float cycleDuration = 30f; // Total duration of a full cycle (in seconds)

    private float cycleTimer = 0f; // Timer to track the cycle progress
    private bool isDay = true; // Is it currently day?

    void Start()
    {
        if (globalLight == null)
        {
            Debug.LogError("Global Light reference is missing!");
        }
    }

    void Update()
    {
        if (globalLight == null) return;

        // Increment the timer
        cycleTimer += Time.deltaTime;

        // Calculate the interpolation factor (0 to 1, then back to 0)
        float t = Mathf.PingPong(cycleTimer / (cycleDuration / 2f), 1f);

        // Lerp between day and night colors
        globalLight.color = Color.Lerp(dayColor, nightColor, t);

        // Reset the cycle timer after a full cycle
        if (cycleTimer > cycleDuration)
        {
            cycleTimer = 0f;
        }
    }
}

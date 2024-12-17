using UnityEngine;

public class FireHandler : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // Flags to track fire states
    private bool isFireActive = false;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing from Fire GameObject!");
        }
    }

    void Update()

    {

        // Automatically manage fire state based on the isDay global variable

        if (!DayNightCycle.isDay && !isFireActive)

        {

            StartFire();

        }

        else if (DayNightCycle.isDay && isFireActive)

        {

            EndFire();

        }

    }


    // Starts the fire by triggering the fire start animation.

    public void StartFire()
    {
        if (animator != null && !isFireActive)
        {
            isFireActive = true;
            animator.Play("FireStart"); // Trigger the fire start animation
            Invoke("PlayFireLoop", 1.0f); // Delay before starting the loop animation (adjust time as needed)
        }
    }


    // Plays the fire loop animation.

    private void PlayFireLoop()
    {
        if (animator != null && isFireActive)
        {
            animator.Play("FireLoop"); // Trigger the fire loop animation
        }
    }


    // Ends the fire by triggering the fire end animation.

    public void EndFire()
    {
        if (animator != null && isFireActive)
        {
            isFireActive = false;

            animator.Play("FireEnd"); // Trigger the fire end animation

        }
    }
}

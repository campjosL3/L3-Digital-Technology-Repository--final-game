// File: DynamicFOV.cs

using UnityEngine;

public class DynamicFOV : MonoBehaviour
{
    // Reference to the Camera component
    public UnityEngine.Camera playerCam;

    // FOV values
    public float normalFOV;
    public float sprintingFOV;
    public float slidingFOV;
    public float fovTransitionSpeed;

    // Reference to the player's movement script
    public PlayerMovement playerMovement;

    void Start()
    {
        // Ensure the camera is assigned
        if (playerCam == null)
        {
            Debug.LogError("Player Camera not assigned. Please assign the PlayerCam in the Inspector.");
        }

        // Ensure the player movement script is assigned
        if (playerMovement == null)
        {
            Debug.LogError("Player Movement script not assigned. Please assign the PlayerMovement in the Inspector.");
        }
    }

    void Update()
    {
        if (playerMovement != null)
        {
            if (playerMovement.isSprinting)
            {
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, sprintingFOV, fovTransitionSpeed * Time.deltaTime);
            }
            else if (playerMovement.isSliding)
            {
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, slidingFOV, fovTransitionSpeed * Time.deltaTime);
            }
            else
            {
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, normalFOV, fovTransitionSpeed * Time.deltaTime);
            }
        }
    }


}


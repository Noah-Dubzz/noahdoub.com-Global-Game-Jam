using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public Transform target; // Reference to the player or target object
    public float height = 20.0f; // The height of the camera above the target
    public float distance = 15.0f; // The distance of the camera behind the target (on Z-axis)

    // LateUpdate is called after all Update functions have been called. 
    // This is ideal for camera movement to prevent jittering.
    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position
            // Start at the target's position
            Vector3 desiredPosition = target.position;
            // Add the height offset
            desiredPosition.y += height;
            // Add the distance offset on the Z-axis
            desiredPosition.z -= distance; 

            // Set the camera's position directly (for a fixed, non-smooth follow)
            transform.position = desiredPosition;

            // Ensure the camera is looking at the target
            transform.LookAt(target.position);
            
            // Optional: If you want a perfectly top-down, non-rotating view, 
            // you can hardcode the rotation
            // transform.rotation = Quaternion.Euler(90, 0, 0); // For a perfectly vertical view
        }
    }
}

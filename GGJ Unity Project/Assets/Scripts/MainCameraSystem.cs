using UnityEngine;

public class MainCameraSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Let's update the camera to not only follow both of the players, but also to zoom in and out based on their distance.
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");   

        if (player1 != null && player2 != null)
        {
            Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2f;
            float distance = Vector3.Distance(player1.transform.position, player2.transform.position);

            // Set camera position to midpoint
            transform.position = new Vector3(midpoint.x, midpoint.y, transform.position.z);

            // Adjust camera zoom based on distance
            Camera camera = GetComponent<Camera>();
            if (camera != null)
            {
                float zoomFactor = Mathf.Clamp(distance, 5f, 20f);
                camera.orthographicSize = zoomFactor;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerprefab;
    [SerializeField] private Transform[] spawnpoints;
    private bool wasdjoined = false;
    private bool arrowsJoined = false;

    

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current == null) return;

        if (!wasdjoined && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerprefab, controlScheme: "WASD", pairWithDevice: Keyboard.current);

            if (spawnpoints.Length > 0)
            {
                player.transform.position = spawnpoints[0].position;
            }
        }
        foreach(var gamePad in Gamepad.all)
        {
            if(gamePad.buttonSouth.wasPressedThisFrame)
            {
                PlayerInput.Instantiate(playerprefab, controlScheme: "GamePad", pairWithDevice:  gamePad);
            }
        }

    }
}

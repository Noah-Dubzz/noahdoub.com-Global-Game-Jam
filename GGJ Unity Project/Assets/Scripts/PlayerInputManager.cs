using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();
    [SerializeField] private GameObject playerprefab;
    [SerializeField] private Transform[] spawnpoints;
    private bool wasdjoined = false;

    private bool gamepadJoined = false;
    

    

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
            wasdjoined = true;
        }
        foreach(var gamePad in Gamepad.all)
        {
            if(gamePad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamePad))
            {
                PlayerInput.Instantiate(playerprefab, controlScheme: "GamePad", pairWithDevice:  gamePad);
                joinedGamepads.Add(gamePad);
            }
        }

    }
}

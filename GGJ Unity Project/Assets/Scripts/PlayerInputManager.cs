using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();
    [SerializeField] private GameObject Harleyprefab;
    [SerializeField] private GameObject Perryprefab;

    [SerializeField] private Transform[] spawnpoints;
    private bool wasdjoined = false;

    private bool gamepadJoined = false;
    

    

    // Update is called once per frame
    void Update()
    {
        gamepadJoined = joinedGamepads.Count > 0;
        if (gamepadJoined)
        {
            if (!wasdjoined && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                if (joinedGamepads.Count < 2)
                {
                    if (!gamepadJoined)
                    {
                        PlayerInput.Instantiate(Harleyprefab, pairWithDevice: Keyboard.current);
                    }
                    else if (joinedGamepads.Count == 1)
                    {
                        PlayerInput.Instantiate(Perryprefab, pairWithDevice: Keyboard.current);
                    }

                    wasdjoined = true;
                }
            }
        }
        
        foreach(var gamePad in Gamepad.all)
        {
            if(gamePad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamePad) && joinedGamepads.Count < 1)
            {
                
                PlayerInput.Instantiate(Harleyprefab, controlScheme: "GamePad", pairWithDevice:  gamePad);
                joinedGamepads.Add(gamePad);
                gamepadJoined = true;

            }
            if (gamePad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamePad) && joinedGamepads.Count == 1)
            {

                PlayerInput.Instantiate(Perryprefab, controlScheme: "GamePad", pairWithDevice: gamePad);
                joinedGamepads.Add(gamePad);
                gamepadJoined = true;

            }

        }

    }
}

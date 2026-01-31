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
       

        
        foreach(var gamePad in Gamepad.all)
        {
            if(gamePad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamePad) && joinedGamepads.Count < 1)
            {
                
                PlayerInput.Instantiate(Harleyprefab, controlScheme: "GamePad", pairWithDevice:  gamePad);
                joinedGamepads.Add(gamePad);

            }
            if (gamePad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamePad) && joinedGamepads.Count == 1)
            {

                PlayerInput.Instantiate(Perryprefab, controlScheme: "GamePad", pairWithDevice: gamePad);
                joinedGamepads.Add(gamePad);

            }

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Jumping & overall speed")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

   





    [Header("Dashing")]
    [SerializeField] private float DashDuration = 2f;
    [SerializeField] private float DashSpeed = 10f;
    [SerializeField] private float DashCooldown = 2f;
    private bool canDash = true;
    private bool isDashing;



    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 velocity;
    private Vector3 dashDirection;
    private Vector3 rollDirection;


    
    //public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");

    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
        if (context.performed & controller.isGrounded)
        {
            Debug.Log("We are supposed to Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

        }
    }

    


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && MaskManager.Instance.HDam)
        {
            Debug.Log(" The player is Rolling ");
            StartCoroutine(RollRoutine());

        }

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

       
        if (isDashing)
        {
            controller.Move(rollDirection * DashSpeed * Time.deltaTime);
            return;
        }

    }


    private IEnumerator RollRoutine()
    {
        canDash = false;
        isDashing = true;

        //animator.SetTrigger("Roll");
        //  the current movement direction at the start of the roll
        rollDirection = new Vector3(moveInput.x, 0, moveInput.y);
        if (rollDirection == Vector3.zero)
        {
            rollDirection = transform.forward;
        }
        // the duration of the roll
        yield return new WaitForSeconds(DashDuration);

        isDashing = false;

        // Start cooldown
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }
}
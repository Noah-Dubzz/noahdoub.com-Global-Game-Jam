using CHAVIS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] HarleyPlayer Harley;
    [SerializeField] PerryPlayer Perry;


    [SerializeField] private GameObject sharp;
    [SerializeField] private GameObject faceAttack;
    [SerializeField] private GameObject faceMove;

    
    [SerializeField] private float TickInterval = 5f;
    [SerializeField] private float Duration = 5f;

    [Header("Jumping & overall speed")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

   





    [Header("Dashing")]
    [SerializeField] private float DashDuration = 2f;
    [SerializeField] private float DashSpeed = 10f;
    [SerializeField] private float DashCooldown = 2f;
    public bool canDash = true;
    private bool isDashing;
    
    

    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 velocity;
    private Vector3 dashDirection;
    private Vector3 rollDirection;
    private Vector2 AimInput;

    public static PlayerMovement Instance;


    //public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        sharp.GetComponent<Collider>();
        this.enabled = true;
    }

  

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");


    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && MaskManager.Instance.Pdam)
        {
            Taunt();
        }
        if (context.performed && canDash && MaskManager.Instance.Psupp)
        {
            Shield();
                HarleyPlayer.Instance.Haura.enabled = true;
            PerryPlayer.Instance.Paura.enabled = true;
        }
    }

    


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && MaskManager.Instance.HDam)
        {
            Debug.Log(" The player is Rolling ");
            StartCoroutine(RollRoutine());

        }
        if (context.performed && canDash && MaskManager.Instance.Hsupp)
        {
            Heal();
        }

        

    }

    public void Taunt()
    {
        Debug.Log("Taunting enemies");
    }

    public void Shield()
    {
        PerryPlayer.Instance.PerryShield += PerryPlayer.Instance.ShieldProtects;
        HarleyPlayer.Instance.HarleyShield += PerryPlayer.Instance.ShieldProtects;

    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
           faceAttack.SetActive(true);
        }
        if(context.canceled)
        {
            faceAttack.SetActive(false);
        }

    }
    public void Aiming(InputAction.CallbackContext context)
    {
        AimInput = context.ReadValue<Vector2>();
    }
        
        // Update is called once per frame
    void Update()
    {
        
        this.enabled = true;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Vector3 Aimings = new ( AimInput.x  ,0, AimInput.y);
        faceMove.transform.position = Aimings += transform.position;

       
        if (isDashing)
        {
            controller.Move(rollDirection * DashSpeed * Time.deltaTime);
            return;
        }
        
    }

    private void  onTriggerEnter (Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //deal damage

            Debug.Log("enemy detected");
        }
    }
    private IEnumerator RollRoutine()
    {
        canDash = false;
        HarleyPlayer.Instance.Damage += HarleyPlayer.Instance.DashDamage;
        isDashing = true;

        //animator.SetTrigger("Roll");
        //  the current movement direction at the start of the roll
        rollDirection = new Vector3(moveInput.x, 0, moveInput.y);
        if (rollDirection == Vector3.zero)
        {
            rollDirection = transform.forward;
            
            
            
        }
        sharp.SetActive(true);
        if (sharp.activeSelf)
        {
            Debug.Log("sharp collider is on");
        }
        // the duration of the roll
        yield return new WaitForSeconds(DashDuration);
        sharp.SetActive(false);
        if(sharp.activeSelf == false)
        {
            Debug.Log("sharp collider is off");
        }

        isDashing = false;

        HarleyPlayer.Instance.Damage -= HarleyPlayer.Instance.DashDamage;
        // Start cooldown
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }

    public void Heal()
    {
        Debug.Log("Heal Started");
        var harley = HarleyPlayer.Instance;
        var perry = PerryPlayer.Instance;

        if (harley == null && perry == null)
        {
            Debug.LogWarning("Heal called but no player instances are present.");
            return;
        }

        bool harleyCanHeal = harley != null && harley.Health < harley.MaxHealth - (harley.MaxHealth * 0.25f);
        bool perryCanHeal = perry != null && perry.Health < perry.MaxHealth - (perry.MaxHealth * 0.25f);

        if (harleyCanHeal || perryCanHeal)
        {
            if (harley != null)
            {
                Debug.Log("Harley: " + harley.Health);
                harley.Health += (harley.MaxHealth * 0.25f);
                Debug.Log("Harley: " + harley.Health);
            }

            if (perry != null)
            {
                Debug.Log("Perry : " + perry.Health);
                perry.Health += Mathf.Round((perry.MaxHealth * 0.25f));
                Debug.Log("Perry : " + perry.Health);
            }

            StartCoroutine(HealthTickH());
        }

        if (perry != null && perry.Health >= perry.MaxHealth)
        {
            perry.Health = perry.MaxHealth;
        }
        if (harley != null && harley.Health >= harley.MaxHealth)
        {
            harley.Health = harley.MaxHealth;
        }

    }
    private IEnumerator HealthTickH()
    {
        int ticks = 0;
        while ( Duration * Time.deltaTime> 0)
        {
            if (ticks < 7 && HarleyPlayer.Instance.Health < HarleyPlayer.Instance.MaxHealth)
            {
                
                HarleyPlayer. Instance.Health += HarleyPlayer.Instance.HealthRegen;
                yield return new WaitForSeconds(TickInterval);
                ticks++;
                
                
            }
            else if (HarleyPlayer.Instance.Health > HarleyPlayer.Instance.MaxHealth)
            {
                HarleyPlayer.Instance.Health = HarleyPlayer.Instance.MaxHealth;
                Debug.Log("break at - Harley: + " + HarleyPlayer.Instance.Health);
                break;
            }
            else
            {
                break;
            }

            if (ticks < 7 && PerryPlayer.Instance.Health < PerryPlayer.Instance.MaxHealth)
            {
                PerryPlayer.Instance.Health += HarleyPlayer.Instance.HealthRegen;
                yield return new WaitForSeconds(TickInterval);
                Debug.Log("perry: + " + PerryPlayer.Instance.Health);
                ticks++;


            }
            else if (PerryPlayer.Instance.Health > PerryPlayer.Instance.MaxHealth)
            {
                PerryPlayer.Instance.Health = PerryPlayer.Instance.MaxHealth;
                Debug.Log("break at - perry: + " + PerryPlayer.Instance.Health);
                break;
            }
            else 
            {
                break;
            }

        }
        yield return new WaitForSeconds(TickInterval);
        canDash = true;
    }
    public void ApplyPU(PowerUpsSO powerUp)
    {

        if (powerUp.powerUpEffect == PowerUpEffect.DashDamageIncrease)
        {
            HarleyPlayer.Instance.DashDamage += powerUp.effectValue1;
        }
        else if (powerUp.powerUpEffect == PowerUpEffect.HealthRegenIncrease)
        {
            HarleyPlayer.Instance.HealthRegen += powerUp.effectValue1;
        }
        else if (powerUp.powerUpEffect == PowerUpEffect.ShieldHealthIncrease)
        {
            PerryPlayer.Instance.shieldHealth += powerUp.effectValue1;
            HarleyPlayer.Instance.shieldHealth += powerUp.effectValue1;
        }
        else if (powerUp.powerUpEffect == PowerUpEffect.TauntDamageIncrease)
        {

        }
        
    }
}
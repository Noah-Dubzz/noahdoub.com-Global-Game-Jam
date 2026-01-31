using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_Projectile : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
    public float innerThreshold = 5f;
    public float outerThreshold = 10f;
    private bool isMoving = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindTargetPlayer();
    }

    void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (isMoving)
            {
                if (distance <= innerThreshold)
                {
                    isMoving = false;
                    rb.linearVelocity = Vector3.zero;
                }
                else
                {
                    rb.linearVelocity = (targetPlayer.transform.position - transform.position).normalized * speed;
                }
            }
            else
            {
                if (distance > outerThreshold)
                {
                    isMoving = true;
                }
                else
                {
                    rb.linearVelocity = Vector3.zero;
                }
            }
        }
    }

    void FindTargetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (Random.value < 0.5f)
        {
            targetPlayer = players[0];
        }
        else
        {
            targetPlayer = players[1];
        }
        
        Debug.Log(targetPlayer.name);
    }
}
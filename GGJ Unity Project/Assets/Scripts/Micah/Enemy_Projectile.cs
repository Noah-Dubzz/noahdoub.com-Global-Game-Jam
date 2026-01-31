using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_Projectile : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
    public float innerThreshold = 5f;
    public float outerThreshold = 10f;
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public float bulletDamage = 5f;
    private float nextFireTime;
    private bool isMoving = true;
    private Rigidbody rb;
    private PerryDamageManager _perryManager;
    private HarleyDamageManager _harleyManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _perryManager = FindAnyObjectByType<PerryDamageManager>();
        _harleyManager = FindAnyObjectByType<HarleyDamageManager>();
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
                    if (Time.time >= nextFireTime)
                    {
                        Fire();
                        nextFireTime = Time.time + fireRate;
                    }
                }
            }
        }
    }

    void FindTargetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null || players.Length == 0)
        {
            targetPlayer = null;
            return;
        }

        if (players.Length == 1)
        {
            targetPlayer = players[0];
            Debug.Log(targetPlayer.name);
            return;
        }

        int index = Random.Range(0, players.Length);
        targetPlayer = players[index];
        Debug.Log(targetPlayer.name);
    }

    void Fire()
    {
        if (bulletPrefab != null && targetPlayer != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
            }
            
            BulletScript bs = bullet.GetComponent<BulletScript>();
            if (bs != null)
            {
                bs.Setup(_perryManager, _harleyManager, bulletDamage);
            }
        }
    }
}
using UnityEngine;
using Micah;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_Projectile : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
    public float health = 100f;
    public float innerThreshold = 5f;
    public float outerThreshold = 10f;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public float bulletDamage = 5f;
    private float _nextFireTime;
    private bool _isMoving = true;
    private Rigidbody _rb;
    private ObjectPool _objectPool;
    private float _maxHealth;
    
    [SerializeField] private SpriteRenderer spritey;
    [SerializeField] private Sprite attacking;
    [SerializeField] private Sprite following;
    
    private PerryDamageManager _perryManager;
    private HarleyDamageManager _harleyManager;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _objectPool = GetComponent<ObjectPool>();
        _maxHealth = health;
    }

    void OnEnable()
    {
        health = _maxHealth;
        _isMoving = true;
        FindTargetPlayer();
    }

    void Start()
    {
        _perryManager = FindAnyObjectByType<PerryDamageManager>();
        _harleyManager = FindAnyObjectByType<HarleyDamageManager>();
    }

    void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (_isMoving)
            {
                if (distance <= innerThreshold)
                {
                    _isMoving = false;
                    _rb.linearVelocity = Vector3.zero;
                }
                else
                {
                    _rb.linearVelocity = (targetPlayer.transform.position - transform.position).normalized * speed;
                }
            }
            else
            {
                if (distance > outerThreshold)
                {
                    _isMoving = true;
                }
                else
                {
                    _rb.linearVelocity = Vector3.zero;
                    if (Time.time >= _nextFireTime)
                    {
                        Fire();
                        _nextFireTime = Time.time + fireRate;
                    }
                }
            }
        }
    }

    void FindTargetPlayer()
    {
        spritey.sprite = following;
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
        spritey.sprite = attacking;
        if (bulletObjPool.Instance != null && targetPlayer != null)
        {
            GameObject bullet = bulletObjPool.Instance.GetBullet(this.transform.position, this.transform.rotation);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            
            Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
                bulletRb.angularVelocity = Vector3.zero;
            }
            
            BulletDamageScript bs = bullet.GetComponent<BulletDamageScript>();
            if (bs != null)
            {
                bs.Setup(_perryManager, _harleyManager, bulletDamage);
            }
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Slash"))
        {
            takeDamage(HarleyPlayer.Instance.Damage);
        }
        if (other.gameObject.CompareTag("AOE"))
        {
            takeDamage(PerryPlayer.Instance.Damage);
        }
    }
    
    void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            if (_objectPool != null)
            {
                _objectPool.ReleaseObject();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
}
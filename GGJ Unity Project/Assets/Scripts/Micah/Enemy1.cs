using CHAVIS;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Enemy1 : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
    public float health = 15f;
    private Rigidbody _rb;
    private ObjectPool _objectPool;
    private float _maxHealth;
    
    [SerializeField] private SpriteRenderer face;
    [SerializeField]private Sprite attack;
    [SerializeField] private Sprite targeting;
    
    private PerryDamageManager _perryManager;
    private HarleyDamageManager _harleyManager;
    
    private int _perryLayer;
    private int _harleyLayer;

    void Awake()
    {
        _perryManager = FindAnyObjectByType<PerryDamageManager>();
        _harleyManager = FindAnyObjectByType<HarleyDamageManager>();
        _objectPool = GetComponent<ObjectPool>();
        _maxHealth = health + (InGameMenus.Instance.waveNumber * 2f);
    }
    
    void OnEnable()
    {
        health = _maxHealth;
        FindTargetPlayer(false);
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        FindTargetPlayer(false);
    }

    void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            Vector3 direction = targetPlayer.transform.position - transform.position;
            direction.y = 0;
            Vector3 moveVelocity = direction.normalized * speed;
            _rb.linearVelocity = new Vector3(moveVelocity.x, 0, moveVelocity.z);
        }
    }

    public void FindTargetPlayer(bool tauntForce)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (tauntForce)
        {
            if (players[0].layer == LayerMask.NameToLayer("Perry"))
            {
                targetPlayer = players[0];
            }
            else
            {
                targetPlayer = players[1];
            }
            return;
        }
        if (players == null || players.Length == 0)
        {
            targetPlayer = null;
            return;
        }

        if (players.Length == 1)
        {
            targetPlayer = players[0];
            //Debug.Log(targetPlayer.name);
            return;
        }

        int index = Random.Range(0, players.Length);
        targetPlayer = players[index];
        //Debug.Log(targetPlayer.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        face.sprite = attack;
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
            AudioManager.Instance?.PlayRedBloodCell();
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
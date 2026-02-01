using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Enemy1 : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
    public float health = 100f;
    private Rigidbody _rb;
    
    private PerryDamageManager _perryManager;
    private HarleyDamageManager _harleyManager;
    
    private int _perryLayer;
    private int _harleyLayer;

    void Awake()
    {
        _perryManager = FindAnyObjectByType<PerryDamageManager>();
        _harleyManager = FindAnyObjectByType<HarleyDamageManager>();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        FindTargetPlayer();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slash"))
        {
            takeDamage(HarleyPlayer.Instance.Damage);
        }
        if (other.gameObject.CompareTag("AOE"))
        {
            takeDamage(HarleyPlayer.Instance.Damage);
        }
    }

    void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
}
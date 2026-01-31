using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Enemy1 : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
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
            Vector3 direction = targetPlayer.transform.position - transform.position;
            direction.y = 0;
            Vector3 moveVelocity = direction.normalized * speed;
            rb.linearVelocity = new Vector3(moveVelocity.x, 0, moveVelocity.z);
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

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player"))
        {
            return;
        }
        //MUST ADD CODE TO CALL A DAMAGE MANAGER
    }
}
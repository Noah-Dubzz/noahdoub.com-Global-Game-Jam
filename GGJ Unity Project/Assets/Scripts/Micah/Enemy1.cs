using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy1 : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public float speed = 10f;
    [SerializeField] private float seekWeight = 1f;
    [SerializeField] private float separationRadius = 1.5f;
    [SerializeField] private float separationWeight = 2f;
    [SerializeField] private float noiseStrength = 0.6f;
    [SerializeField] private float noiseSpeed = 1.2f;
    [SerializeField] private LayerMask enemyLayer = ~0;
    private Rigidbody rb;
    private float noiseSeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        noiseSeed = Random.Range(0f, 1000f);
        FindTargetPlayer();
    }

    void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            Vector3 direction = targetPlayer.transform.position - transform.position;
            direction.y = 0;
            Vector3 seek = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector3.zero;

            Vector3 separation = Vector3.zero;
            int neighborCount = 0;
            Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius, enemyLayer);
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i].attachedRigidbody == rb)
                {
                    continue;
                }

                Enemy1 other = neighbors[i].GetComponentInParent<Enemy1>();
                if (other == null)
                {
                    continue;
                }

                Vector3 away = transform.position - other.transform.position;
                away.y = 0;
                float distSqr = away.sqrMagnitude;
                if (distSqr > 0.001f)
                {
                    separation += away.normalized / Mathf.Sqrt(distSqr);
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                separation /= neighborCount;
            }

            float noiseX = Mathf.PerlinNoise(noiseSeed, Time.time * noiseSpeed) * 2f - 1f;
            float noiseZ = Mathf.PerlinNoise(noiseSeed + 10f, Time.time * noiseSpeed) * 2f - 1f;
            Vector3 noise = new Vector3(noiseX, 0f, noiseZ) * noiseStrength;

            Vector3 desired = (seek * seekWeight) + (separation * separationWeight) + noise;
            if (desired.sqrMagnitude < 0.001f)
            {
                desired = seek;
            }

            Vector3 moveVelocity = desired.normalized * speed;
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
}
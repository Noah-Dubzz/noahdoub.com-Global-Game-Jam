using System.Collections;
using UnityEngine;

namespace CHAVIS
{
    public class Enemy : MonoBehaviour
    {
        [Header("Basic Stats")]
        private Transform target;
        public float speed = 10f;
        public float fireRate = 0.3f;
        public float health = 10;
        public int score = 100;
        public float powerUpDropChance = 1f;
        public Hero playerHealth;
        public float damage = 1f;
        public float projDamage;

        [Header("Projectile Enemy Stats")]
        public GameObject enemyProjectile;
        public float projSpeed = 10f;
        public LayerMask IsPlayer;
        public float attackCooldown;
        public float nextAttackTime;
        bool hasAttacked;
        public float attackRange;
        bool inRange = false;
        public Rigidbody body;
        // removed RegularProjectileEnemy dependency

        [Header("Homing Enemy Stats")]
        public float timeToExplode;
        public float waitExplosion;
        public GameObject spawnPosition;
        public GameObject collisionExplosion;
        public float timeToDestroy;

        // removed BoundsChecks usage — no longer required
        private WaveSpawner waveSpawner;

        void Awake()
        {
            waveSpawner = GetComponentInParent<WaveSpawner>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


        }

        //public Vector3 pos { get { return this.transform.position; } set { this.transform.position = value; } }

        // Update is called once per frame
        void FixedUpdate()
        {
            // bounds checks removed — rely on WaveSpawner or other systems to cleanup
            if (gameObject.CompareTag("MeleeEnemy"))
            {
                Move();
            }
            else
            {
                inRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
                if (inRange)
                {
                    Attack();
                }
                else if (!inRange)
                {
                    Move();
                }
            }
        }

        public virtual void Move()
        {
            transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
        }
        void OnCollisionEnter(Collision coll)
        {
            GameObject otherGO = coll.gameObject;
            ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
            if (p != null)
            {
                if (otherGO.GetComponent<ProjectileHero>() != null)
                {
                    health -= Hero.Instance.damage;
                    if (health <= 0)
                    {
                        Destroy(otherGO);
                            Destroy(this.gameObject);
                            if (waveSpawner != null && waveSpawner.waves != null && waveSpawner.waves.Length > 0)
                            {
                                int idx = (waveSpawner.currentWaveNumber - 1) % waveSpawner.waves.Length;
                                Debug.Log(waveSpawner.waves[idx].enemiesLeft);
                                waveSpawner.waves[idx].enemiesLeft = waveSpawner.waves[idx].enemiesLeft - 1;
                                Debug.Log(waveSpawner.waves[idx].enemiesLeft);
                            }
                    }
                }
                Destroy(otherGO);
            }
            else if (coll.gameObject.tag == "Player")
            {
                playerHealth = coll.gameObject.GetComponent<Hero>();
                playerHealth.TakeDamage(damage);
                //Debug.Log("Enemy hit by non-ProjectileHero: " + otherGO.name);
            }
        }
        private void Attack()
        {
            if (gameObject.CompareTag("RegProjEnemy"))
            {
                body.linearVelocity = Vector3.zero;


                if (!hasAttacked)
                {
                    GameObject projGO = Instantiate<GameObject>(enemyProjectile);
                    projGO.transform.position = transform.position;

                    Vector3 direction = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().position - projGO.transform.position;
                    Vector3 rotation = projGO.transform.position - GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().position;

                    Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
                    rigidB.linearVelocity = new Vector3(direction.x, direction.y, 0).normalized * projSpeed;
                    float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                    projGO.transform.rotation = Quaternion.Euler(0, 0, rot + 90);
                    hasAttacked = true;

                    //Debug.Log("Enemy is Shooting");

                    Invoke(nameof(ResetAttack), attackCooldown);
                }
            }
            else if (gameObject.CompareTag("HomProjEnemy"))
            {
                body.linearVelocity = Vector3.zero;


                if (!hasAttacked)
                {
                    GameObject projGO = Instantiate(enemyProjectile, spawnPosition.transform.position, enemyProjectile.transform.rotation);

                    StartCoroutine(Homing(projGO));

                    hasAttacked = true;

                    //Debug.Log("Enemy is Shooting");

                    Invoke(nameof(ResetAttack), attackCooldown);
                }
            }
        }
        public IEnumerator Homing(GameObject projGO)
        {
            while (Vector3.Distance(target.position, projGO.transform.position) >= 0f)
            {
                projGO.transform.position += (target.position - projGO.transform.position).normalized * projSpeed * Time.deltaTime;

                projGO.transform.LookAt(target.position);

                if (Vector3.Distance(target.position, projGO.transform.position) <= 0f)
                {

                    GameObject explosion = (GameObject)Instantiate(collisionExplosion, projGO.transform.position, transform.rotation);
                    Destroy(projGO, timeToDestroy);
                    Destroy(explosion, 3f);
                }
                else
                {

                    Destroy(projGO, timeToDestroy);

                }

                yield return null;
            }
            Destroy(projGO);
        }
        private void ResetAttack()
        {

            if (nextAttackTime <= attackCooldown + Time.time)
            {
                //Debug.Log("Attack on cooldown");
                hasAttacked = false;
            }
        }
    }
}

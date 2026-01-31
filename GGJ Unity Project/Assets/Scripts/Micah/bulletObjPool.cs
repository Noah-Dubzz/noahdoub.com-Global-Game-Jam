using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Micah
{
    public class bulletObjPool : MonoBehaviour
    {
        public static bulletObjPool Instance;

        public GameObject bulletPrefab;
        public int poolSize = 20;
        private Queue<GameObject> _pool = new Queue<GameObject>();

        void Awake()
        {
            Instance = this;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                _pool.Enqueue(bullet);
            }
        }

        public GameObject GetBullet(Vector3 position, Quaternion rotation)
        {
            GameObject bullet = _pool.Dequeue();
            
            bullet.SetActive(false); 
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true);
            
            _pool.Enqueue(bullet);

            return bullet;
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }
}

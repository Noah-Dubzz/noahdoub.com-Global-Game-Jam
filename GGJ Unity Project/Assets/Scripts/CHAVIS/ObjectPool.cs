using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    private IObjectPool<ObjectPool> enemyPool;
    public static ObjectPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPool(IObjectPool<ObjectPool> pool)
    {
        enemyPool = pool;
    }
    public void ReleaseObject()
    {
        enemyPool.Release(this);
    }
}

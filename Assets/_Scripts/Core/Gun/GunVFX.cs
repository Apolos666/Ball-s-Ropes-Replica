using System;
using Apolos.SO;
using UnityEngine;
using UnityEngine.Pool;

public class GunVFX : MonoBehaviour
{
    private ObjectPool<GunVFX> _objectPool;
    
    public void SetPool(ObjectPool<GunVFX> pool)
    {
        _objectPool = pool;
    }

    private void OnParticleSystemStopped()
    {
        _objectPool.Release(this);
    }
}

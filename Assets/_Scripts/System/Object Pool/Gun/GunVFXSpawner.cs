using System;
using Apolos.SO;
using UnityEngine;
using UnityEngine.Pool;

public class GunVFXSpawner : MonoBehaviour
{
    [SerializeField] private GunRecoilZone _gunRecoilZone;
    private ObjectPool<GunVFX> _objectPool;
    [SerializeField] private GunVFX _gunVFXPrefab;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 100;

    private void Awake()
    {
        _objectPool = new ObjectPool<GunVFX>(CreateGunVFX, OnTakeGunVFXFromPool, OnReturnGunVFXToPool, OnDestroyGunVFX, true, _defaultCapacity, _maxSize);
    }

    private void OnEnable()
    {
        _gunRecoilZone.OnGunCollision += OnGunCollision;
    }

    private void OnDisable()
    {
        _gunRecoilZone.OnGunCollision -= OnGunCollision;
    }

    private void OnGunCollision()
    {
        _objectPool.Get();
    }

    private void OnDestroyGunVFX(GunVFX gunVFX)
    {
        Destroy(gunVFX.gameObject);
    }

    private void OnReturnGunVFXToPool(GunVFX gunVFX)
    {
        gunVFX.gameObject.SetActive(false);
    }

    private void OnTakeGunVFXFromPool(GunVFX gunVFX)
    {
        gunVFX.gameObject.SetActive(true);
        var particle = gunVFX.GetComponentInChildren<ParticleSystem>();
        particle.Play();
    }
    
    private GunVFX CreateGunVFX()
    {
        var gunVFX = Instantiate(_gunVFXPrefab, transform.position, Quaternion.identity, transform);
        gunVFX.gameObject.SetActive(false);
        gunVFX.SetPool(_objectPool);
        return gunVFX;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Apolos.Core;
using Apolos.System;
using Apolos.System.EventManager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class BaseBallSpawner : MonoBehaviour
{
    protected ObjectPool<BallOld> _pool;
    
    [FormerlySerializedAs("_ball")]
    [Header("Spawner setting")]
    [SerializeField] protected BallOld BallOld;
    [SerializeField] protected int _defaultCapacity = 10;
    [SerializeField] protected int _maxSize = 20;
    [SerializeField] protected int _ballsPerWave;
    [SerializeField] protected float _ballSpawnRate;
    [SerializeField] private PhysicMaterial _ballInPipe;
    [SerializeField] protected AudioClip _respawnClip;
    protected List<BallOld> _currentBalls = new List<BallOld>();
    protected Coroutine _currentCoroutine;
    
    private bool IsResetSpawner = true;
    [SerializeField] private float _resetTimerCooldown = 2f;
    private float _resetTimer = 0f;

    private bool _isCompleteLevel;

    protected void Awake()
    {
        _pool = new ObjectPool<BallOld>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, true, _defaultCapacity, _maxSize);
        EventManager.AddListener("LevelCompleted", () => _isCompleteLevel = true);
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _resetTimer += Time.deltaTime;

        if (_resetTimer >= _resetTimerCooldown)
        {
            IsResetSpawner = true;
        }
    }

    public void IsResetAble()
    {
        if (!IsResetSpawner) return;
        
        ResetBall();
        IsResetSpawner = false;
        _resetTimer = 0f;
    }

    protected IEnumerator SpawnBallOverTime()
    {
        for (int i = 0; i < _ballsPerWave; i++)
        {
            var ball = _pool.Get();
            _currentBalls.Add(ball);

            if (i == _ballsPerWave - 1)
            {
                yield return new WaitUntil(() => ball.IsRelease);
                
                if (_isCompleteLevel)
                {
                    break;
                }
                
                AudioManager.Instance.PlaySound(_respawnClip);
                _currentCoroutine = StartCoroutine(SpawnBallOverTime());
                break;
            }

            yield return new WaitForSeconds(_ballSpawnRate);
        }
    }
    
    protected void ResetBall()
    {
        for (int i = 0; i < _currentBalls.Count; i++)
        {
            _currentBalls[i].gameObject.SetActive(false);
        }

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        StartCoroutine(SpawnBallOverTime());
    }
    
    private void OnDestroyBall(BallOld ballOld)
    {
        Destroy(ballOld.gameObject);
    }

    private void OnReturnBallToPool(BallOld ballOld)
    {
        ballOld.gameObject.layer = LayerMask.NameToLayer("Default");
        ballOld.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballOld.GetComponent<Collider>().sharedMaterial = _ballInPipe;
        ballOld.transform.position = transform.position;
        ballOld.transform.rotation = quaternion.identity;
        ballOld.GetComponent<TrailRenderer>().Clear();
        ballOld.gameObject.SetActive(false);
    }

    private void OnTakeBallFromPool(BallOld ballOld)
    {
        ballOld.IsRelease = false;
        ballOld.GetComponent<TrailRenderer>().enabled = true;
        
        ballOld.gameObject.SetActive(true);
    }

    private BallOld CreateBall()   
    {
        BallOld ballOld = Instantiate(BallOld, transform.position, Quaternion.identity, transform);
        
        ballOld.SetPool(_pool);
        
        return ballOld;
    }
}

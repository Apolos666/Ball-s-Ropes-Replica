using System;
using System.Collections;
using System.Collections.Generic;
using Apolos.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class BaseBallSpawner : MonoBehaviour
{
    protected ObjectPool<Ball> _pool;
    
    [Header("Spawner setting")]
    [SerializeField] protected Ball _ball;
    [SerializeField] protected int _defaultCapacity = 10;
    [SerializeField] protected int _maxSize = 20;
    [SerializeField] protected int _ballsPerWave;
    [SerializeField] protected float _ballSpawnRate;
    protected List<Ball> _currentBalls = new List<Ball>();
    protected Coroutine _currentCoroutine;
    
    private bool IsResetSpawner = true;
    [SerializeField] private float _resetTimerCooldown = 2f;
    private float _resetTimer = 0f;

    protected void Awake()
    {
        _pool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, true, _defaultCapacity, _maxSize);
    }

    private void Update()
    {
        UpgradeTimer();
    }

    private void UpgradeTimer()
    {
        _resetTimer += Time.deltaTime;
        
        print(_resetTimer);

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
                // print("Ball at the end" + Count++);
                _currentCoroutine = StartCoroutine(SpawnBallOverTime());
                break;
            }

            yield return new WaitForSeconds(_ballSpawnRate);
        }
    }
    
    private void ResetBall()
    {
        for (int i = 0; i < _currentBalls.Count; i++)
        {
            _currentBalls[i].gameObject.SetActive(false);
        }
        
        StopCoroutine(_currentCoroutine);

        StartCoroutine(SpawnBallOverTime());
    }
    
    private void OnDestroyBall(Ball ball)
    {
        Destroy(ball.gameObject);
    }

    private void OnReturnBallToPool(Ball ball)
    {
        ball.gameObject.SetActive(false);
    }

    private void OnTakeBallFromPool(Ball ball)
    {
        ball.transform.position = transform.position;
        ball.transform.rotation = quaternion.identity;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.IsRelease = false;
        
        ball.gameObject.SetActive(true);
    }

    private Ball CreateBall()   
    {
        Ball ball = Instantiate(_ball, transform.position, Quaternion.identity, transform);
        
        ball.SetPool(_pool);
        
        return ball;
    }
}

using System;
using System.Collections;
using Apolos.Core;
using Apolos.SO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class BallSpawner : MonoBehaviour
{
    private ObjectPool<Ball> _pool;
    [Header("Spawner setting")]
    [SerializeField] private Ball _ball;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private int _ballsPerWave;
    [SerializeField] private float _ballSpawnRate;

    [Header("Listening to event")] 
    [SerializeField] private VoidEventChannelSO _onAddBall;

    private void Awake()
    {
        _pool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, true, _defaultCapacity, _maxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnBallOverTime());
    }

    private void OnEnable()
    {
        _onAddBall.OnEventRaised += AddBall;
    }

    private void OnDisable()
    {
        _onAddBall.OnEventRaised -= AddBall;
    }

    private IEnumerator SpawnBallOverTime()
    {
        for (int i = 0; i < _ballsPerWave; i++)
        {
            var ball = _pool.Get();

            if (i == _ballsPerWave - 1)
            {
                yield return new WaitUntil(() => ball.IsRelease);
                break;
            }

            yield return new WaitForSeconds(_ballSpawnRate);
        }
    
        StartCoroutine(SpawnBallOverTime());
    }

    private void AddBall()
    {
        _pool.Get();
        _ballsPerWave++;
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

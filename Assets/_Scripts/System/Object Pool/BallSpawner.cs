using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private VoidEventChannelSO _onMergeBall;

    private List<Ball> _currentBalls = new List<Ball>();
    private Coroutine _currentCoroutine;
    private int Count;
    private bool IsResetSpawner = true;
    [SerializeField] private float _resetTimerCooldown = 2f;
    private float _resetTimer = 0f;

    private void Awake()
    {
        _pool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, false, _defaultCapacity, _maxSize);
    }

    private void Start()
    {
        _currentCoroutine = StartCoroutine(SpawnBallOverTime()); 
        _onAddBall.OnEventRaised += OnAddBall;
        _onMergeBall.OnEventRaised += OnMergeBall;
    }

    private void OnMergeBall()
    {
        
    }

    private void OnDestroy()
    {
        _onAddBall.OnEventRaised -= OnAddBall;
        _onMergeBall.OnEventRaised -= OnMergeBall;
    }

    private void OnAddBall()
    {
        var ball = _pool.Get();
        _ballsPerWave++;
    }

    public IEnumerator SpawnBallOverTime()
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

    public void ResetBall()
    {
        for (int i = 0; i < _currentBalls.Count; i++)
        {
            _currentBalls[i].gameObject.SetActive(false);
        }
        
        StopCoroutine(_currentCoroutine);

        StartCoroutine(SpawnBallOverTime());
    }

    private void Update()
    {
        _resetTimer += Time.deltaTime;
        print(_resetTimer);

        if (_resetTimer >= _resetTimerCooldown)
        {
            IsResetSpawner = true;
        }
        
        if (Input.GetKeyDown(KeyCode.A) && IsResetSpawner)
        {
            ResetBall();
            IsResetSpawner = false;
            _resetTimer = 0f;
        }
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

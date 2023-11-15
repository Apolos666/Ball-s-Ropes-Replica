using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apolos.Core;
using Apolos.SO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class BallSpawner : MonoBehaviour
{
    private ObjectPool<BallOld> _pool;
    [FormerlySerializedAs("_ball")]
    [Header("Spawner setting")]
    [SerializeField] private BallOld _ballOld;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private int _ballsPerWave;
    [SerializeField] private float _ballSpawnRate;

    [Header("Listening to event")] 
    [SerializeField] private VoidEventChannelSO _onAddBall;
    [SerializeField] private VoidEventChannelSO _onMergeBall;

    private List<BallOld> _currentBalls = new List<BallOld>();
    private Coroutine _currentCoroutine;
    private int Count;
    private bool IsResetSpawner = true;
    [SerializeField] private float _resetTimerCooldown = 2f;
    private float _resetTimer = 0f;

    private void Awake()
    {
        _pool = new ObjectPool<BallOld>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, false, _defaultCapacity, _maxSize);
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

    private void OnDestroyBall(BallOld ballOld)
    {
        Destroy(ballOld.gameObject);
    }

    private void OnReturnBallToPool(BallOld ballOld)
    {
        ballOld.gameObject.SetActive(false);
    }

    private void OnTakeBallFromPool(BallOld ballOld)
    {
        ballOld.transform.position = transform.position;
        ballOld.transform.rotation = quaternion.identity;
        ballOld.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballOld.IsRelease = false;
        
        ballOld.gameObject.SetActive(true);
    }

    private BallOld CreateBall()   
    {
        BallOld ballOld = Instantiate(_ballOld, transform.position, Quaternion.identity, transform);
        
        ballOld.SetPool(_pool);
        
        return ballOld;
    }
}

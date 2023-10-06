using System.Collections;
using Apolos.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class BallSpawner : MonoBehaviour
{
    private ObjectPool<Ball> _pool;
    [SerializeField] private Ball _ball;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private int _ballsPerWave;
    [SerializeField] private float _ballSpawnRate;

    private void Awake()
    {
        _pool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, true, _defaultCapacity, _maxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnBallOverTime());
    }

    private IEnumerator SpawnBallOverTime()
    {
        for (int i = 0; i < _ballsPerWave; i++)
        {
            var ball = _pool.Get();
            ball.IsRelease = false;

            if (i == _ballsPerWave - 1)
            {
                yield return new WaitUntil(() => ball.IsRelease);
            }
            else
            {
                yield return new WaitForSeconds(_ballSpawnRate);
            }
        }
        
        print("Ball Release");
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
        
        ball.gameObject.SetActive(true);
    }

    private Ball CreateBall()   
    {
        Ball ball = Instantiate(_ball, transform.position, Quaternion.identity, transform);
        
        ball.SetPool(_pool);
        
        return ball;
    }
}

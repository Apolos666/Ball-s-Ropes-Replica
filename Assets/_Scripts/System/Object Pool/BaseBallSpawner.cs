using System.Collections;
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
    
    protected void Awake()
    {
        _pool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, true, _defaultCapacity, _maxSize);
    }
    
    protected IEnumerator SpawnBallOverTime()
    {
        if (_ballsPerWave == 0)
        {
           
        }
        else
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
        }

        yield return new WaitForSeconds(1);
        
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

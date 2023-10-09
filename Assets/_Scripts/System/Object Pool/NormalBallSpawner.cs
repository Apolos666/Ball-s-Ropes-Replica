using Apolos.SO;
using UnityEngine;

public class NormalBallSpawner : BaseBallSpawner
{
    [Header("Listening to event")] 
    [SerializeField] private VoidEventChannelSO _onAddBall;
    [SerializeField] private IntEventChannelSO _onChangeBallsPerWave;
    
    private void Start()
    {
        _currentCoroutine = StartCoroutine(SpawnBallOverTime());
        _onAddBall.OnEventRaised += OnAddBall;
    }

    private void OnDestroy()
    {
        _onAddBall.OnEventRaised -= OnAddBall;
    }

    public void OnAddBall()
    {
        var ball = _pool.Get();
        _ballsPerWave++;
    }
    
    public bool IsUpgrable()
    {
        return _ballsPerWave - 3 >= 1;
    }
    
    public void SubtractToUpgrade()
    {
        _ballsPerWave -= 3;
        _onChangeBallsPerWave.RaiseEvent(_ballsPerWave);
    }
}

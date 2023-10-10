using Apolos.SO;
using UnityEngine;

public class NormalBallSpawner : BaseBallSpawner
{
    [Header("Listening to event")] 
    [SerializeField] private VoidEventChannelSO _onAddBall;

    [SerializeField] private VoidEventChannelSO _onChangedBallsPerWave;

    [Header("Ball Configuration")] 
    [SerializeField] private int _requiredBallsToUpgrade = 3;
    
    private void Start()
    {
        _currentCoroutine = StartCoroutine(SpawnBallOverTime());
        _onAddBall.OnEventRaised += OnAddBall;
    }

    private void OnDestroy()
    {
        _onAddBall.OnEventRaised -= OnAddBall;
    }

    private void OnAddBall()
    {
        var ball = _pool.Get();
        _ballsPerWave++;
        _onChangedBallsPerWave.RaiseEvent();
    }
    
    public bool IsUpgrable()
    {
        return _ballsPerWave - _requiredBallsToUpgrade >= 1;
    }
    
    public void SubtractToUpgrade()
    {
        _ballsPerWave -= _requiredBallsToUpgrade;
        _onChangedBallsPerWave.RaiseEvent();
        ResetBall();
    }
}

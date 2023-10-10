using Apolos.SO;
using UnityEngine;

public class BetterBallSpawner : BaseBallSpawner
{
    [Header("Listening to event")] 
    [SerializeField] private VoidEventChannelSO _onMergeBall;
    [SerializeField] private NormalBallSpawner _normalBallSpawner;

    private void OnEnable()
    {
        _onMergeBall.OnEventRaised += MergeBall;
    }

    private void OnDisable()
    {
        _onMergeBall.OnEventRaised -= MergeBall;
    }

    private void MergeBall()
    {
        if (_normalBallSpawner.IsUpgrable())
        {
            _normalBallSpawner.SubtractToUpgrade();
            _ballsPerWave++;
            if (_ballsPerWave == 1)
            {
                _currentCoroutine = StartCoroutine(SpawnBallOverTime());
            }
        }
    }
}

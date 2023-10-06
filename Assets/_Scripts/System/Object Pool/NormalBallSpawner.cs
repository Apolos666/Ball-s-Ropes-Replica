using System.Threading.Tasks;
using Apolos.Core;
using Apolos.SO;
using UnityEngine;

public class NormalBallSpawner : BaseBallSpawner
{
    [Header("Listening to event")] 
    [SerializeField] private VoidEventChannelSO _onAddBall;
    [SerializeField] private IntEventChannelSO _onChangeBallsPerWave;
    
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

    private async void AddBall()
    {
        Ball ball = _pool.Get();

        while (!ball.IsRelease)
        {
            await Task.Delay(100);
        }
        
        _ballsPerWave++;
        _onChangeBallsPerWave.RaiseEvent(_ballsPerWave);
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

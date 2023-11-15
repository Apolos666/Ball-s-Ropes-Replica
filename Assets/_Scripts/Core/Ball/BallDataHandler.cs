using UnityEngine;

public class BallDataHandler : MonoBehaviour
{
    [Header("Ball Configuration")]
    [SerializeField] private int _point;

    private Ball _ball;

    private void Awake()
    {
        SetUp();
    }

    private void SetUp()
    {
        _ball = new(_point);
    }

    public Ball GetBall => _ball;
}

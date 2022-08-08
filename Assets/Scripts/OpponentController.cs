using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private PaddleController _paddle;
    [SerializeField] private BallController _ball;

    public void Initialize(GameController gameController, PaddleController paddle, BallController ball)
    {
        _gameController = gameController;
        _paddle = paddle;
        _ball = ball;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var ballPos = _ball.transform.position.y;
        var diff = ballPos - _paddle.transform.position.y;
        var ballDir = _ball.Velocity.y;

        if (Mathf.Sign(ballDir) != Mathf.Sign(diff))
        {
            if (diff < _paddle.Height)
            {
                _paddle.Move(0.0f);
                return;
            }
        }

        float tol = 0.5f;
        float percentage = Mathf.InverseLerp(0, tol, Mathf.Abs(diff));

        _paddle.Move(percentage * Mathf.Sign(diff));
    }
}

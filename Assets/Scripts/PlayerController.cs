using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum Movement
    {
        Idle,
        Up,
        Down
    }

    private Movement _movement;

    [SerializeField] private GameController _gameController;
    [SerializeField] private PaddleController _paddle;
    [SerializeField] private BallController _ball;

    public void Initialize(GameController gameController, PaddleController paddle, BallController ball)
    {
        _gameController = gameController;
        _paddle = paddle;
        _ball = ball;
    }

    private void Update()
    {
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        if (up && !down)
        {
            _movement = Movement.Up;
        }
        else if (!up && down)
        {
            _movement = Movement.Down;
        }
        else {
            _movement = Movement.Idle;
        }
    }

    private void FixedUpdate()
    {
        switch (_movement)
        {
            case Movement.Up:
                _paddle.MoveUp(1);
                break;
            case Movement.Down:
                _paddle.MoveDown(1);
                break;
            case Movement.Idle:
                _paddle.Move(0);
                break;
        }
    }
    
}

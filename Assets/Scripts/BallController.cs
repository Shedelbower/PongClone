using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private AudioClip _paddleHitClip;
    [SerializeField] private AudioClip _wallHitClip;

    private float _bounceRandomness = 0.5f;
    private float _resetDuration = 1.0f;
    private bool _isReseting = false;
    private GameController _gameController;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AudioSource _audioSource;
    private float _speed;

    public Vector2 Velocity => _rb.velocity;

    private float _resetTimer = 0.0f;

    private readonly Vector2 _maxAngleVector = (new Vector2(1, 2)).normalized;

    public bool Active
    {
        get => _active;
        set
        {
            _active = value;
            if (!_active)
            {
                _cachedVelocity = _rb.velocity;
                _rb.velocity = Vector2.zero;
            }
            else
            {
                _rb.velocity = _cachedVelocity;
            }
        }
    }

    private bool _active;
    private Vector2 _cachedVelocity;

    public void Initialize(GameController gameController, float speed)
    {
        _gameController = gameController;
        _speed = speed;
        Active = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Paddle")
        {
            var randomness = _bounceRandomness * Random.Range(-1f,1f);
            _rb.AddForce(Vector2.up * randomness, ForceMode2D.Impulse);

            var paddle = collision.transform.parent.gameObject.GetComponent<PaddleController>();
            _rb.AddForce(paddle.Velocity, ForceMode2D.Impulse);

            _audioSource.PlayOneShot(_paddleHitClip);
        }
        else if (collision.collider.tag == "Wall")
        {
            if (collision.gameObject.name == "Left")
            {
                _gameController.OnOpponentScored();
            }
            else if (collision.gameObject.name == "Right")
            {
                _gameController.OnPlayerScored();
            }
            else
            {
                _audioSource.PlayOneShot(_wallHitClip);
            }
        }
    }

    private void Update()
    {
        if (_isReseting && this.Active)
        {
            _resetTimer += Time.deltaTime;
            if (_resetTimer > _resetDuration)
            {
                var rvel = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                _rb.velocity = rvel;
                _isReseting = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isReseting)
        {
            var maxDot = Vector2.Dot(_maxAngleVector, Vector2.up);
            var vdot = Mathf.Abs(Vector2.Dot(_rb.velocity.normalized, Vector2.up));

            if (vdot > maxDot)
            {
                var vel = _rb.velocity;
                var x = Mathf.Sign(vel.x) * _maxAngleVector.x;
                var y = Mathf.Sign(vel.y) * _maxAngleVector.y;
                _rb.velocity = new Vector2(x,y);
            }

            if (_rb.velocity.sqrMagnitude - 0.01f > _speed * _speed || _rb.velocity.sqrMagnitude + 0.01f < _speed * _speed)
            {
                _rb.velocity = _rb.velocity.normalized * _speed;
            }
        }
        
    }

    public void Reset()
    {
        this.transform.position = Vector3.zero;
        _rb.velocity = Vector2.zero;
        _resetTimer = 0.0f;
        _isReseting = true;
    }

}

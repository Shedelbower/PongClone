using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameSettings _settings;
    [SerializeField] private PlayerController _player;
    [SerializeField] private OpponentController _opponent;
    [SerializeField] private PaddleController _playerPaddle;
    [SerializeField] private PaddleController _opponentPaddle;
    [SerializeField] private BallController _ball;
    [SerializeField] private UIController _uiController;

    [SerializeField] private string _menuSceneName;
    [SerializeField] private GameObject _scoreEffectPrefab;


    public bool IsPaused
    {
        get;
        private set;
    }

    public bool GameIsOver
    {
        get;
        private set;
    }


    private void Initialize(GameSettings settings)
    {
        this.IsPaused = false;
        
        // Agents
        _player.Initialize(this, _playerPaddle, _ball);
        _opponent.Initialize(this, _opponentPaddle, _ball);

        // Paddles
        _playerPaddle.Initialize(_settings.playerSpeed, _settings.playerPaddleHeight);
        _opponentPaddle.Initialize(_settings.opponentSpeed, _settings.opponentPaddleHeight);

        // Ball
        _ball.Initialize(this, _settings.ballSpeed);

        // UI
        _uiController.Initialize(this, _settings.scoreToWin);

        PointReset();
    }

    public void OnPlayerScored() => OnBallScored(true);
    public void OnOpponentScored() => OnBallScored(false);
    public void OnBallScored(bool playerScored)
    {
        var go = Instantiate<GameObject>(_scoreEffectPrefab, _ball.transform.position, Quaternion.identity);
        var lookat = _ball.transform.position;
        lookat.x = 0.0f;
        go.transform.LookAt(lookat);
        
        PointReset();

        if (playerScored)
        {
            _uiController.OnPlayerScored();
        }
        else 
        {
            _uiController.OnOpponentScored();
        }

        if (_uiController.PlayerScore >= _settings.scoreToWin || _uiController.OpponentScore >= _settings.scoreToWin)
        {
            EndGame();
        }
    }

    private void PointReset()
    {
        _ball.Reset();
    }

    private void TogglePause()
    {
        if (!GameIsOver)
        {
            this.IsPaused = !this.IsPaused;

            _playerPaddle.Active = !this.IsPaused;
            _opponentPaddle.Active = !this.IsPaused;
            _ball.Active = !this.IsPaused;
            _uiController.SetPauseVisibility(this.IsPaused);
        }
    }

    private void Awake()
    {
        Initialize(_settings);
    }

    private void EndGame()
    {
        var playerWon = _uiController.PlayerScore >= _settings.scoreToWin;
        _uiController.SetGameOverVisibility(true);
        if (!IsPaused)
        {
            TogglePause();
        }

        _uiController.SetPauseVisibility(false);

        GameIsOver = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (GameIsOver && Input.anyKeyDown)
        {
            SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Single);
        }
    }
}

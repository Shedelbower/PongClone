using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerScoreLabel;
    [SerializeField] private TextMeshProUGUI _opponentScoreLabel;
    [SerializeField] private TextMeshProUGUI _gameOverLabel;

    [SerializeField] private GameObject _scoreBase;
    [SerializeField] private GameObject _gameOverBase;
    [SerializeField] private GameObject _pauseBase;

    public int PlayerScore => _playerScore;
    public int OpponentScore => _opponentScore;

    private GameController _gameController;
    private int _scoreLimit;
    private int _playerScore;
    private int _opponentScore;

    public void Initialize(GameController gameController, int scoreLimit)
    {
        _gameController = gameController;
        _scoreLimit = scoreLimit;

        _playerScore = 0;
        _opponentScore = 0;

        SetScore(_playerScoreLabel, _playerScore);
        SetScore(_opponentScoreLabel, _opponentScore);

        _pauseBase.SetActive(false);
        _gameOverBase.SetActive(false);
    }

    public void SetPauseVisibility(bool visible)
    {
        _pauseBase.SetActive(visible);
    }

    public void SetGameOverVisibility(bool visible)
    {
        _gameOverBase.SetActive(visible);
        bool playerWon = _playerScore >= _scoreLimit;
        _gameOverLabel.text = playerWon ? "You Won!" : "You Lost";
    }

    public void OnPlayerScored()
    {
        _playerScore++;
        SetScore(_playerScoreLabel, _playerScore);
    }

    public void OnOpponentScored()
    {
        _opponentScore++;
        SetScore(_opponentScoreLabel, _opponentScore);
    }

    private void SetScore(TextMeshProUGUI label, int score)
    {
        label.text = score.ToString();
    }
}

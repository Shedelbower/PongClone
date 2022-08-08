using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Custom/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    // Player
    public float playerSpeed = 10f;
    public float playerPaddleHeight = 5f;

    // Opponent
    public float opponentSpeed = 10f;
    public float opponentPaddleHeight = 5f;

    // Ball
    public float ballSpeed = 8f;

    // Score
    public int scoreToWin = 11;
}

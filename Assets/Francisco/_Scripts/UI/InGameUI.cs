using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InGameUI: MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI player_1Score, player_2Score, timeText;

    [Header("UiButton")]
    [SerializeField] Button pauseBtn;

    [Header("Timer")]
    private double time;

    private void Start()
    {
        pauseBtn.onClick.AddListener(PauseGame); //Button to pause game
    }
    private void OnEnable()
    {
        player_1Score.text = GameManager.gameManager.GetPlayerInfo()[0]["PlayerScore"].ToString(); //Gets player 1 score
        player_2Score.text = GameManager.gameManager.GetPlayerInfo()[1]["PlayerScore"].ToString(); //Gets player 2 score
        timeText.text = GameManager.gameManager.GetTimer().ToString(); //make time apeears on the screen

    }

    private void Update()
    {
        if(GameManager.gameManager.State == GameState.InGame) 
        {
            time = GameManager.gameManager.GetTimer(); //Gets the game Time
            timeText.text = Math.Floor(time).ToString(); //Makes the current time appear on the screen
            player_1Score.text = GameManager.gameManager.GetPlayerInfo()[0]["PlayerScore"].ToString();//Gets player1 current score
            player_2Score.text = GameManager.gameManager.GetPlayerInfo()[1]["PlayerScore"].ToString();//Gets player2 current score
        }
        
    }

    private void PauseGame() { GameManager.gameManager.UpdateGameState(GameState.Pause); }
}

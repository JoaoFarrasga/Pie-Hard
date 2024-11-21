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
        pauseBtn.onClick.AddListener(PauseGame);
    }
    private void OnEnable()
    {
        player_1Score.text = GameManager.gameManager.GetPlayerInfo()[0]["PlayerScore"].ToString();
        player_2Score.text = GameManager.gameManager.GetPlayerInfo()[1]["PlayerScore"].ToString();
        timeText.text = GameManager.gameManager.GetTimer().ToString();

    }

    private void Update()
    {
        if(GameManager.gameManager.State == GameState.InGame) 
        {
            time = GameManager.gameManager.GetTimer();
            timeText.text = Math.Floor(time).ToString();
            player_1Score.text = GameManager.gameManager.GetPlayerInfo()[0]["PlayerScore"].ToString();
            player_2Score.text = GameManager.gameManager.GetPlayerInfo()[1]["PlayerScore"].ToString();
        }
        
    }

    private void PauseGame() { GameManager.gameManager.UpdateGameState(GameState.Pause); }
}

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

    

    private void Start()
    {
        pauseBtn.onClick.AddListener(PauseGame);
    }
    private void OnEnable()
    {
        player_1Score.text = GameManager.gameManager.GetPlayerInfo()[0]["PlayerScore"].ToString();
        player_2Score.text = GameManager.gameManager.GetPlayerInfo()[1]["PlayerScore"].ToString();
        timeText.text = UIManager.uiManager.GetTimer().ToString();

    }

    private void Update()
    {
        //time -= Time.deltaTime;
        //timeText.text = Math.Floor(time).ToString();
    }

    private void PauseGame() { GameManager.gameManager.UpdateGameState(GameState.Pause); }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> uiGO = new();
    private GameObject currentUI;
    private GameState lastGameState;
    [SerializeField] GameObject countDownUI;
    private Coroutine videoCoroutine;


    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateUIChanged;
        currentUI = uiGO[0]; // Makes main menu current ui
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateUIChanged;
    }

    private void OnGameStateUIChanged(GameState gameState)
    {
        // Cancela a corrotina do v�deo caso ela esteja ativa (para evitar conflitos)
        if (videoCoroutine != null)
        {
            StopCoroutine(videoCoroutine);
            videoCoroutine = null;
        }

        switch (gameState)
        {
            case GameState.InitialScreen:
                DisableAllUI();
                EnableUIGO(0);
                break;
            case GameState.VideoPlayer:
                DisableAllUI();
                EnableUIGO(4); // Ativa o v�deo
                videoCoroutine = StartCoroutine(VideoPlayerTimer());
                break;
            case GameState.GameStart:
                DisableUIGO();
                countDownUI.SetActive(true);
                break;
            case GameState.InGame:
                if (currentUI.activeSelf) DisableUIGO();
                EnableUIGO(1);
                break;
            case GameState.GameEnd:
                break;
            case GameState.ShowResults:
                DisableUIGO();
                EnableUIGO(3);
                break;
            case GameState.Pause:
                EnableUIGO(2);
                break;
            default:
                break;
        }

        lastGameState = gameState;
    }

    private void EnableUIGO(int ui) // Changes UI according to the game state
    {
        currentUI = uiGO[ui];
        currentUI.SetActive(true);
    }

    private void DisableUIGO() { currentUI.SetActive(false); } // Disables current UI

    private void DisableAllUI()
    {
        foreach (GameObject ui in uiGO) { ui.SetActive(false); }
    }

    // Corrotina para mudar o estado automaticamente ap�s 11 segundos
    private IEnumerator VideoPlayerTimer()
    {
        yield return new WaitForSeconds(11f); // Aguarda 11 segundos
        GameManager.gameManager.UpdateGameState(GameState.GameStart); // Atualiza o estado para GameStart
    }
}

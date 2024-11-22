using UnityEngine;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> uiGO = new();
    private GameObject currentUI;
    private GameState lastGameState;


    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateUIChanged;
        currentUI = uiGO[0]; //Makes main menu current ui
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateUIChanged;
    }

    private void OnGameStateUIChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.InitialScreen:
                DisableUIGO();
                EnableUIGO(0);
                break;
            case GameState.GameStart:
                break;
            case GameState.InGame:
                DisableUIGO();
                EnableUIGO(1);
                break;
            case GameState.GameEnd:
                break;
            case GameState.Pause:
                EnableUIGO(2);
                break;
            default:
                break;
        }

        lastGameState = gameState;
    }

    //Enables Current UI GameObjects
    public void EnableUIGO(int ui) //Changes ui according to the game state
    {
        currentUI = uiGO[ui];
        currentUI.SetActive(true);
    }

    //Disables Current UI GameObjects
    public void DisableUIGO() { currentUI.SetActive(false); } //Disables current ui
}

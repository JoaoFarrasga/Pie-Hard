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
        Debug.Log("ChangingUI To: " + gameState);
        switch (gameState)
        {
            case GameState.InitialScreen:
                DisableAllUI();
                EnableUIGO(0);
                break;
            case GameState.GameStart:
                DisableUIGO();
                break;
            case GameState.InGame:
                if(currentUI.activeSelf) DisableUIGO();
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

    //Enables Current UI GameObjects
    private void EnableUIGO(int ui) //Changes ui according to the game state
    {
        currentUI = uiGO[ui];
        currentUI.SetActive(true);
    }

    //Disables Current UI GameObjects
    private void DisableUIGO() { currentUI.SetActive(false); } //Disables current ui

    private void DisableAllUI()
    {
        foreach(GameObject ui in uiGO) { ui.SetActive(false); }
    }


}

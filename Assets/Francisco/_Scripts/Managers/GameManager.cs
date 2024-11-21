using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    //private Dictionary<string, int> playerScore = new();

    private List<Dictionary<string, int>> players = new();

    [Header("Timer")]
    [SerializeField] double time = 60;


    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        State = GameState.InitialScreen;
        //UpdateGameState(GameState.InitialScreen);
        AddPlayers(2);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.InitialScreen:
                UIManager.uiManager.DisableUIGO();
                UIManager.uiManager.EnableUIGO(0);
                ResetTimer();
                ResetScore();
                break;
            case GameState.GameStart:
                UIManager.uiManager.DisableUIGO();
                UIManager.uiManager.EnableUIGO(1);
                break;
            case GameState.InGame:
                UIManager.uiManager.DisableUIGO();
                UIManager.uiManager.EnableUIGO(1);
                break;
            case GameState.GameEnd:
                break;
            case GameState.Pause:
                UIManager.uiManager.DisableUIGO();
                UIManager.uiManager.EnableUIGO(2);
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void Update()
    {
        if(State == GameState.InGame) time -= Time.deltaTime;
    }

    public void AddPlayers(int numOfPlayers) 
    {
        for(int i = 0; i < numOfPlayers; i++)
        {
            Dictionary<string, int> player = new();
            player.Add("PlayerID", i + 1);
            player.Add("PlayerScore", 0);
            players.Add(player);
        }
        Debug.Log("ID: " + players[0]["PlayerID"] + " Score: " + players[0]["PlayerScore"]);
        Debug.Log("ID: " + players[1]["PlayerID"] + " Score: " + players[1]["PlayerScore"]);
    }


    public void OnScoreChanged(int playerID)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i]["PlayerID"] == playerID) players[i]["PlayerScore"]++;
            Debug.Log("ID: " + players[i]["PlayerID"] + "Score: " + players[i]["PlayerScore"]);
        }

    }

    public List<Dictionary<string, int>> GetPlayerInfo()
    {
        return players;
    }

    public double GetTimer() { return time; }

    public void ResetTimer() { time = 60; }

    public void ResetScore()
    {
        for (int i = 0; i < players.Count; i++) { players[i]["PlayerScore"] = 0; }
    }

}

public enum GameState
{
    InitialScreen,
    GameStart,
    InGame, 
    GameEnd,
    Pause
}

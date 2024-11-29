using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private List<Dictionary<string, int>> players = new();
    private Dictionary<string, int> winner;

    [Header("Timer")]
    [SerializeField] double time = 15;


    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            Destroy(gameObject);// If an instance already exists, destroy this one
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.VideoPlayer);
        AddPlayers(2);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.InitialScreen:
                break;
            case GameState.VideoPlayer:
                StartCoroutine(PlayVideoAndSwitchState());
                break;
            case GameState.GameStart:
                StartCoroutine(StartGame());
                break;
            case GameState.InGame:
                break;
            case GameState.GameEnd:
                FindWinner();
                break;
            case GameState.ShowResults:
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void Update()
    {
        if (State == GameState.InGame) time -= Time.deltaTime; // Game Timer
        if (time <= 0) StartCoroutine(EndGame());
    }

    // Add the exact number of players to the dictionary to store the score and player IDs
    public void AddPlayers(int numOfPlayers)
    {
        for (int i = 0; i < numOfPlayers; i++)
        {
            Dictionary<string, int> player = new();
            player.Add("PlayerID", i + 1);
            player.Add("PlayerScore", 0);
            players.Add(player);
        }
    }

    // Score changes when a projectile hits a player
    public void OnScoreChanged(int playerID)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i]["PlayerID"] == playerID) players[i]["PlayerScore"]++;
        }
    }

    private IEnumerator PlayVideoAndSwitchState()
    {
        // Aguarde at� que o v�deo termine (11 segundos neste caso)
        yield return new WaitForSeconds(11f);
        UpdateGameState(GameState.InitialScreen); // Troca para o pr�ximo estado
    }

    private IEnumerator StartGame()
    {
        ResetScore();
        yield return new WaitForSeconds(3f);
        UpdateGameState(GameState.InGame);
    }

    private IEnumerator EndGame()
    {
        ResetTimer();
        UpdateGameState(GameState.GameEnd);
        yield return new WaitForSeconds(3f);
        UpdateGameState(GameState.ShowResults);
    }

    private void FindWinner()
    {
        int points = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i]["PlayerScore"] > points)
            {
                points = players[i]["PlayerScore"];
                winner = players[i];
            }
            else if (players[i]["PlayerScore"] == points) winner = null;
        }
    }

    // Returns dictionary with player info
    public List<Dictionary<string, int>> GetPlayerInfo() { return players; }

    // Returns time of the timer
    public double GetTimer() { return time; }

    // Resets the timer
    public void ResetTimer() { time = 45; }

    // Resets the score
    public void ResetScore()
    {
        for (int i = 0; i < players.Count; i++) { players[i]["PlayerScore"] = 0; }
    }

    public Dictionary<string, int> GetWinner() { return winner; }

}

// States of the game
public enum GameState
{
    InitialScreen,
    VideoPlayer, // Novo estado
    GameStart,
    InGame,
    GameEnd,
    ShowResults,
    Pause
}

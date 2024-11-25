using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{

    [Header("UiButton")]
    [SerializeField] Button mainMenu;
    [SerializeField] Button restartBtn;

    private void Start()
    {
        mainMenu.onClick.AddListener(GoToMainMenu);
        restartBtn.onClick.AddListener(RestartGame);//Button to pause game
    }

    private void GoToMainMenu() { GameManager.gameManager.UpdateGameState(GameState.InitialScreen); }
    private void RestartGame() { GameManager.gameManager.UpdateGameState(GameState.InGame); }
}

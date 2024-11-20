using UnityEngine;
using UnityEngine.UI;

public class PauseScreenUI : MonoBehaviour
{
    [Header("UIButtons")]
    [SerializeField] Button resumeBtn, mainMenuBtn, quitGameBtn;

    private void Start()
    {
        resumeBtn.onClick.AddListener(ResumeGame);
        mainMenuBtn.onClick.AddListener(BackToMainMenu);
        quitGameBtn.onClick.AddListener(QuitGame);
    }

    private void ResumeGame() { GameManager.gameManager.UpdateGameState(GameState.InGame); }

    private void BackToMainMenu() { GameManager.gameManager.UpdateGameState(GameState.InitialScreen); }

    private void QuitGame() { Application.Quit(); }
}

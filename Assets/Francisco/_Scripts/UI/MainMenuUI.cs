using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button startGameBtn, settingsBtn, quitGameBtn;

    private void Start()
    {
        startGameBtn.onClick.AddListener(StartGame);
        settingsBtn.onClick.AddListener(SettingsScreen);
        quitGameBtn.onClick.AddListener(QuitGame);
    }

    private void StartGame() 
    {
        GameManager.gameManager.UpdateGameState(GameState.InGame); 
    }
        
    private void SettingsScreen()
    {

    } 

    private void QuitGame() { Application.Quit();  }
}

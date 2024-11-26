using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject countDownUI;

    [Header("UI Buttons")]
    [SerializeField] private Button startGameBtn, settingsBtn, quitGameBtn;

    private void Start()
    {
        startGameBtn.onClick.AddListener(StartGame);
        settingsBtn.onClick.AddListener(SettingsScreen);     //Clicks buttons 
        quitGameBtn.onClick.AddListener(QuitGame);
    }

    private void StartGame() 
    {
        GameManager.gameManager.UpdateGameState(GameState.GameStart);
        countDownUI.SetActive(true);
    }
        
    private void SettingsScreen()
    {

    } 

    private void QuitGame() { Application.Quit();  }
}

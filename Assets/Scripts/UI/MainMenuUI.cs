using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button startGameBtn, settingsBtn, quitGameBtn;

    private void Start()
    {
        Debug.Log("1");
        startGameBtn.onClick.AddListener(StartGame);
        settingsBtn.onClick.AddListener(SettingsScreen);     //Clicks buttons 
        quitGameBtn.onClick.AddListener(QuitGame);
    }

    private void StartGame() 
    {
        Debug.Log("2");
        GameManager.gameManager.UpdateGameState(GameState.InGame); 
    }
        
    private void SettingsScreen()
    {

    } 

    private void QuitGame() { Application.Quit();  }
}

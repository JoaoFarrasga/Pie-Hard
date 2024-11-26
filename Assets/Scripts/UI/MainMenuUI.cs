using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject countDownUI;

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
<<<<<<< Updated upstream
        Debug.Log("2");
        GameManager.gameManager.UpdateGameState(GameState.InGame); 
=======
        GameManager.gameManager.UpdateGameState(GameState.GameStart);
        countDownUI.SetActive(true);
>>>>>>> Stashed changes
    }
        
    private void SettingsScreen()
    {

    } 

    private void QuitGame() { Application.Quit();  }
}

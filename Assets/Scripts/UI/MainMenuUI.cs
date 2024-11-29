using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [Header("UI Buttons")]
    [SerializeField] private Button startGameBtn, settingsBtn, quitGameBtn;

    private bool check = false;

    private void Start()
    {
        startGameBtn.onClick.AddListener(StartGame);
        settingsBtn.onClick.AddListener(SettingsScreen);     //Clicks buttons 
        quitGameBtn.onClick.AddListener(QuitGame);
    }

    private void StartGame() 
    {
        //if (check == false)
        //{
        //    GameManager.gameManager.UpdateGameState(GameState.VideoPlayer);
        //    check = true;
        //}
        //else
            GameManager.gameManager.UpdateGameState(GameState.GameStart);
    }
        
    private void SettingsScreen()
    {

    } 

    private void QuitGame() { Application.Quit();  }
}

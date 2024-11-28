using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] GameObject countDownUI;

    [SerializeField] Color blueColor, redColor;

    [Header("UiButton")]
    [SerializeField] Button mainMenu;
    [SerializeField] Button restartBtn;

    private void Start()
    {
        mainMenu.onClick.AddListener(GoToMainMenu);
        restartBtn.onClick.AddListener(RestartGame);//Button to Restart game
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");
        if (GameManager.gameManager.GetWinner() != null)
        {
            transform.Find("WinnerText").gameObject.GetComponent<TextMeshProUGUI>().text = "Winner";
            TextMeshProUGUI playerColorText = transform.Find("PlayerColorText").gameObject.GetComponent<TextMeshProUGUI>();
            if (GameManager.gameManager.GetWinner()["PlayerID"] == 1)
            {
                playerColorText.text = "Blue";
                playerColorText.color = blueColor;
            }
            else
            {
                playerColorText.text = "Red";
                playerColorText.color = redColor;
            }
        }
        else transform.Find("PlayerColorText").gameObject.GetComponent<TextMeshProUGUI>().text = "Draw";
    }

    private void GoToMainMenu() { GameManager.gameManager.UpdateGameState(GameState.InitialScreen); }
    private void RestartGame() 
    { 
        GameManager.gameManager.UpdateGameState(GameState.GameStart);
        countDownUI.SetActive(true);
    }
}

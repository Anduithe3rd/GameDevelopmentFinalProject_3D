using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameOverManager : MonoBehaviour
{
    public GameObject endScreenPanel;
    public TextMeshProUGUI endText; // Or TextMeshProUGUI if you're using TMP
    public Button restartButton;
    public Button quitButton;

    public void ShowEndScreen(bool didWin)
    {
        //FindObjectOfType<PlayerDisabler>()?.DisableAllExceptCamera();

        endScreenPanel.SetActive(true);
        endText.text = didWin ? "You Win!" : "You Died";

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => Application.Quit());
    }

}

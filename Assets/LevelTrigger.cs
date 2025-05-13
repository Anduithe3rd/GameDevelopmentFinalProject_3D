using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public enum TriggerType { Win, Lose }
    public TriggerType triggerType = TriggerType.Lose;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (triggerType == TriggerType.Lose)
        {
            Debug.Log("You lost!");

            // Restart the current scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            FindObjectOfType<GameOverManager>().ShowEndScreen(false);
            // Uncomment this to load a specific scene instead
            // SceneManager.LoadScene("LoseScene");
        }
        else if (triggerType == TriggerType.Win)
        {
            Debug.Log("You win!");
            FindObjectOfType<GameOverManager>().ShowEndScreen(true);

            // Uncomment this to load a specific victory screen
            // SceneManager.LoadScene("VictoryScene");
        }
    }
}

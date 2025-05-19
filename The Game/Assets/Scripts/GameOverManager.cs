using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class GameOverManager : MonoBehaviour
    {
        public void RetryGame()
        {
            // Unpause the game
            Time.timeScale = 1f;
            
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReturnToMainMenu()
        {
            // Unpause the game
            Time.timeScale = 1f;
            
            // Load the Main Menu scene
            SceneManager.LoadScene("MainMenu");
        }

        public void CloseGameOverPanel()
        {
            // Hide the Game Over panel
            gameObject.SetActive(false);
        }
    }
}
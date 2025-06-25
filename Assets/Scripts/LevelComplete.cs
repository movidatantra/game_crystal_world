using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public int levelToUnlock = 2;
    public string mainMenuSceneName = "MainMenu"; // Sesuaikan dengan nama scene MainMenu kamu

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cek dan update progress
            int currentLevel = PlayerPrefs.GetInt("LevelUnlocked", 1);
            if (currentLevel < levelToUnlock)
            {
                PlayerPrefs.SetInt("LevelUnlocked", levelToUnlock);
                PlayerPrefs.Save();
                Debug.Log("Level " + levelToUnlock + " terbuka!");
            }

            // Kembali ke Main Menu
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}

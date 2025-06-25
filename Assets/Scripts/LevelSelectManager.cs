using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject levelPanel;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        levelPanel.SetActive(false); // panel awalnya disembunyikan

        int unlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        level1Button.interactable = true;
        level2Button.interactable = unlocked >= 2;
        level3Button.interactable = unlocked >= 3;
    }

    public void ShowLevelPanel()
    {
        levelPanel.SetActive(true);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progress direset.");
    }
}

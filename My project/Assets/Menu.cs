using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    public Button[] levelButtons;
    public Button backButton;

    public Button startButton;
    public Button selectLevelsButton;
    public Button quitButton;

    private void Start()
    {
        levelSelectPanel.SetActive(false);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToMainMenu);
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(() => SceneManager.LoadScene("Level1"));
        }

        if (selectLevelsButton != null)
        {
            selectLevelsButton.onClick.AddListener(OpenLevelSelect);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }

    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}

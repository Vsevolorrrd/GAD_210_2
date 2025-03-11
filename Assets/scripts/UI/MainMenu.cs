using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    PauseMode pause;
    public void Sandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void BattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}

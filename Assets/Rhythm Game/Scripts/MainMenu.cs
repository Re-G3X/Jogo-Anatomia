using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string dialogueSceneName;
    [SerializeField] private string creditsSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void PlayGame()
    {
        SceneLoader.Instance.LoadScene(dialogueSceneName);
    }

    public void ShowCredits()
    {
        SceneLoader.Instance.LoadScene(creditsSceneName);
    }

    public void BackToMenu()
    {
        SceneLoader.Instance.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

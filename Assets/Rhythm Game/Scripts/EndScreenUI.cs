using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string menuSceneName;

    public void RestartGame()
    {
        SceneLoader.Instance.LoadScene(gameSceneName);
    }

    public void GoToMenu()
    {
        SceneLoader.Instance.LoadScene(menuSceneName);
    }
}
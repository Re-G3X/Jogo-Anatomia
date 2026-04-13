using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrocarCena : MonoBehaviour
{
    [SerializeField] private string scene;

    public void ChangeToScene()
    {
        SceneManager.LoadScene(this.scene);
    }

    public void ChangeToGameplay(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

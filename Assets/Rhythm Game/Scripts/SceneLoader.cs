using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private float delay = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }

    private IEnumerator Load(string sceneName)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    void Start()
    {
        if (instance != null)
        {
            Debug.Log("Attempting to create more than one SceneLoader!");
            return;
        }

        instance = this;
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);
    }

    public void OpenGame()
    {
        SceneManager.UnloadSceneAsync("Title");
        SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
    }
}

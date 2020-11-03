using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int currentSceneIndex;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(++currentSceneIndex);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}

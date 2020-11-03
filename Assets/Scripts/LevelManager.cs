using System;
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
        if(currentSceneIndex+1 >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
            currentSceneIndex = 0;           
        }
        else
        {
            SceneManager.LoadScene(++currentSceneIndex);
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}

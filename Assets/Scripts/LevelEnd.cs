using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(FindObjectOfType<PlayerController2D>().gameObject);
        FindObjectOfType<LevelManager>().LoadNextScene();
    }
}

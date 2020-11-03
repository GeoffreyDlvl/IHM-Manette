using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(PlayVictory());
    }

    IEnumerator PlayVictory()
    {
        FindObjectOfType<FeedbackManager>().PlayFeedback(FeedbackManager.CharacterAction.WIN);
        yield return new WaitForSeconds(0.8f);
        Destroy(FindObjectOfType<PlayerController2D>().gameObject);
        FindObjectOfType<LevelManager>().LoadNextScene();
    }
}

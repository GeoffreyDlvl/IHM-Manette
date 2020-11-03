using System;
using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    Transform respawnPosition;

    [SerializeField]
    float respawnDelay = 2f;

    GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController2D>().gameObject;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        player.SetActive(false);
        player.transform.position = respawnPosition.position;
        player.GetComponent<PlayerController2D>().ResetPlayer();
        yield return new WaitForSeconds(respawnDelay);
        player.SetActive(true);
    }

    internal void UpdateRespawnPosition(Transform transform)
    {
        this.respawnPosition = transform;
    }
}

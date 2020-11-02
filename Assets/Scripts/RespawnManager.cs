﻿using System.Collections;
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
        Renderer renderer = player.GetComponent<Renderer>();
        player.SetActive(false);
        player.transform.position = respawnPosition.position;
        yield return new WaitForSeconds(respawnDelay);
        player.SetActive(true);
    }
}
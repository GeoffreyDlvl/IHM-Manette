using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    RespawnManager respawnManager;

    private void Awake()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UpdateLightRenderer();
        UpdateRespawnPosition();
    }

    private void UpdateRespawnPosition()
    {
        respawnManager.UpdateRespawnPosition(transform);
    }

    private void UpdateLightRenderer()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Light"))
            {
                child.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }    
    }
}

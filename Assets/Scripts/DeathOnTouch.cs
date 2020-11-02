using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeathOnTouch : MonoBehaviour
{
    RespawnManager respawnManager;

    // Start is called before the first frame update
    void Awake()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("death");
        if (other.gameObject.CompareTag("Player"))
        {
            respawnManager.Respawn();
        }
    }
}

using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    Transform respawnPosition;

    GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController2D>().gameObject;
    }

    public void Respawn()
    {
        player.transform.position = respawnPosition.position;
    }
}

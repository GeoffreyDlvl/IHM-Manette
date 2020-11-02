using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    AudioClip checkpointSFX;

    RespawnManager respawnManager;
    bool activated = false;

    private void Awake()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated)
        {
            UpdateLightRenderer();
            UpdateRespawnPosition();
            PlayCheckpointSFX();
            activated = true;  
        }
    }

    private void PlayCheckpointSFX()
    {
        AudioSource cameraAudioSource;
        cameraAudioSource = Camera.main.GetComponent<AudioSource>();
        cameraAudioSource.clip = checkpointSFX;
        cameraAudioSource.Play();
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

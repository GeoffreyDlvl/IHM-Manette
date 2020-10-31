using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public enum CharacterAction
    {
        DASH,
        JUMP,
        RUN
    }

    #region Particles
    [SerializeField]
    private ParticleSystem dashParticleSystemPrefab;
    private ParticleSystem dashParticleSystem;
    #endregion

    #region SFX
    [SerializeField]
    AudioClip jumpSFX;
    #endregion

    AudioSource cameraAudioSource;
    PlayerController2D playerController;

    private void Awake()
    {
        cameraAudioSource = Camera.main.GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController2D>();

        dashParticleSystem = Instantiate(dashParticleSystemPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1), Quaternion.identity);
        dashParticleSystem.transform.parent = transform;
    }

    public void PlayFeedback(CharacterAction action)
    {
        switch(action)
        {
            case CharacterAction.DASH:
                if (!dashParticleSystem.isPlaying)
                {
                    var mainModule = dashParticleSystem.main;
                    mainModule.duration = playerController.GetDashDuration();
                    var shapeModule = dashParticleSystem.shape;
                    shapeModule.rotation = new Vector3(0, 0, (int) playerController.orientation == 1 ? 270 : 90);
                    dashParticleSystem.Play();
                }                    
                break;
            case CharacterAction.JUMP:
                cameraAudioSource.clip = jumpSFX;
                cameraAudioSource.volume = .3f;
                cameraAudioSource.Play();
                break;
            default:
                break;
        }
    }
}

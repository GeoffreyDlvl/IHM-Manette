using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public enum CharacterAction
    {
        DASH,
        JUMP,
        RUN,
        FLIP
    }

    #region Particles
    [SerializeField]
    private ParticleSystem dashParticleSystemPrefab;
    private ParticleSystem dashParticleSystem;

    [SerializeField]
    ParticleSystem dustParticles;
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
            case CharacterAction.FLIP:
                CreateDust();
                break;
            default:
                break;
        }
    }

    private void CreateDust()
    {
        dustParticles.Play();
    }    
}

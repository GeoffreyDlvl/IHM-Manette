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
    ParticleSystem dashParticles;
    //private ParticleSystem dashParticleSystem;

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

        ParticleSystem.MainModule dashMainModule = dashParticles.main;
        dashMainModule.duration = playerController.GetDashDuration();
    }

    public void PlayFeedback(CharacterAction action)
    {
        switch(action)
        {
            case CharacterAction.DASH:
                CreateDashEffect();                
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

    private void CreateDashEffect()
    {
        ParticleSystem.VelocityOverLifetimeModule velocityModule = dashParticles.velocityOverLifetime;
        velocityModule.x = (-1) * (int)playerController.orientation * 5;
        dashParticles.Play();
        dustParticles.Play();
    }

    private void Update()
    {
        var velocityOverLifetime = dashParticles.velocityOverLifetime;
    }

    private void CreateDust()
    {
        dustParticles.Play();
    }    
}

﻿using System;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public enum CharacterAction
    {
        DASH,
        JUMP,
        RUN,
        FLIP,
        WALLDRAG
    }

    #region Particles
    [SerializeField]
    ParticleSystem dashParticles;

    [SerializeField]
    ParticleSystem dustParticles;
    #endregion

    #region SFX
    [SerializeField]
    AudioClip jumpSFX;
    
    [SerializeField]
    AudioClip dashSFX;

    [SerializeField]
    AudioClip dragSFX;
    #endregion

    AudioSource cameraAudioSource;
    PlayerController2D playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController2D>();

        cameraAudioSource = Camera.main.GetComponent<AudioSource>();

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
                CreateJumpEffect();
                break;
            case CharacterAction.FLIP:
                CreateDust();
                break;
            case CharacterAction.WALLDRAG:
                CreateWallDragEffect();
                break;
            default:
                break;
        }
    }

    private void CreateWallDragEffect()
    {
        if (! cameraAudioSource.isPlaying)
        {
            cameraAudioSource.clip = dragSFX;
            cameraAudioSource.Play();
        }
        if (playerController.Velocity.y > 0f)
        {
            cameraAudioSource.Stop();
        }
    }

    private void CreateJumpEffect()
    {
        cameraAudioSource.clip = jumpSFX;
        cameraAudioSource.volume = .3f;
        cameraAudioSource.Play();

        CreateDust();
    }

    private void CreateDashEffect()
    {
        cameraAudioSource.clip = dashSFX;
        cameraAudioSource.volume = .3f;
        cameraAudioSource.Play();

        ParticleSystem.VelocityOverLifetimeModule velocityModule = dashParticles.velocityOverLifetime;
        velocityModule.x = (-1) * (int)playerController.orientation * 5;
        dashParticles.Play();
        
        if (playerController.IsGrounded)
        {
            CreateDust();
        }
    }

    private void CreateDust()
    {
        dustParticles.Play();
    }
}

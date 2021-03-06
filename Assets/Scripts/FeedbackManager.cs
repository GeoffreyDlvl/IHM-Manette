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
        DIE,
        WALLDRAG,
        WIN
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
    AudioClip dieSFX;

    [SerializeField]
    AudioClip dragSFX;


    [SerializeField]
    AudioClip winSFX;
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
            case CharacterAction.DIE:
                CreateDieAction();
                break;
            case CharacterAction.WALLDRAG:
                CreateWallDragEffect();
                break;
            case CharacterAction.WIN:
                createWinAction();
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

    private void CreateDieAction() {
        cameraAudioSource.clip = dieSFX;
        cameraAudioSource.volume = .3f;
        cameraAudioSource.Play();
    }

    private void createWinAction()
    {
        cameraAudioSource.clip = winSFX;
        cameraAudioSource.volume = .3f;
        cameraAudioSource.Play();
    }
}

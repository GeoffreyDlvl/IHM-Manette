using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    private void Awake()
    {
        cameraAudioSource = Camera.main.GetComponent<AudioSource>();

        dashParticleSystem = Instantiate(dashParticleSystemPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1), Quaternion.identity);
        dashParticleSystem.transform.parent = transform;
    }

    public void StartFeedbackActionOf(CharacterAction action, float duration)
    {
        switch(action)
        {
            case CharacterAction.DASH:
                if (!dashParticleSystem.isPlaying)
                {
                    var mainModule = dashParticleSystem.main;
                    mainModule.duration = duration;
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

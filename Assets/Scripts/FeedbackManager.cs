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

    private void Awake()
    {
        cameraAudioSource = Camera.main.GetComponent<AudioSource>();

        dashParticleSystem = Instantiate(dashParticleSystemPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1), Quaternion.identity);
        dashParticleSystem.transform.parent = transform;
    }

    public void StartFeedbackActionOf(CharacterAction action, float duration, int particlEmissionDirection)
    {
        switch(action)
        {
            case CharacterAction.DASH:
                if (!dashParticleSystem.isPlaying)
                {
                    var mainModule = dashParticleSystem.main;
                    mainModule.duration = duration;
                    var shapeModule = dashParticleSystem.shape;
                    shapeModule.rotation = new Vector3(0,0, particlEmissionDirection == 1 ? 270 : 90);
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

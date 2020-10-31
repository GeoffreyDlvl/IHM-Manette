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

    private void Awake()
    {
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
            default:
                break;
        }
    }
}

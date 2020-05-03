using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVisuals : MonoBehaviour
{
    [SerializeField] bool useGameObject;
    [SerializeField] bool affectColliderOnActivation;
    [SerializeField] bool affectColliderOnDeactivation;
    
    ParticleSystem[] particleSystems;
    Collider2D projectileCollider;
    Tween tween;

    bool useTween;
    AbilityComponent abilityComponent;

    Action activateAction;
    Action deactivateAction;

    [HideInInspector]
    public bool isActiveOnScreen;

    // Start is called before the first frame update
    void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();

        if (projectileCollider == null)
            Debug.LogError("ProjectileVisuals Awake() projectileCollider == null");

        particleSystems = GetComponentsInChildren<ParticleSystem>();
        DeactivateParticleSystems();

        tween = GetComponent<AbilitySpawnTween>();
        if (tween)
            useTween = true;
        else
            useTween = false;

        abilityComponent = GetComponent<AbilityComponent>();
        abilityComponent.enabled = false;

        SetActions();
    }

    void SetActions()
    {
        if(useGameObject)
        {
            activateAction = new Action(ActivateGameObject);
            deactivateAction = new Action(DeactivateGameObject);
        }
        else
        {
            if (affectColliderOnActivation)
            {
                if (useTween)
                    activateAction = new Action(ActivateAndTween);
                else
                    activateAction = new Action(ActivateColliderAndParticles);
            }
            else
                activateAction = new Action(ActivateParticleSystems);

            if (affectColliderOnDeactivation)
                deactivateAction = new Action(DeactivateColliderAndParticles);
            else
                deactivateAction = new Action(DeactivateParticleSystems);
        }
    }

    public bool IsActiveOnScreen()
    {
        return isActiveOnScreen;
    }

    public void Activate()
    {
        activateAction.Invoke();
        abilityComponent.enabled = true;
        isActiveOnScreen = true;
    }

    public void Deactivate()
    {
        deactivateAction.Invoke();
        abilityComponent.enabled = false;
        isActiveOnScreen = false;
    }

    void ActivateAndTween()
    {
        ActivateParticleSystems();
        tween.Execute();
    }

    void DeactivateColliderAndParticles()
    {
        projectileCollider.enabled = false;
        DeactivateParticleSystems();
    }

    void ActivateColliderAndParticles()
    {
        projectileCollider.enabled = true;
        ActivateParticleSystems();
    }

    void ActivateParticleSystems()
    {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Play();
        }
    }

    void DeactivateParticleSystems()
    {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Stop();
        }
    }

    void ActivateGameObject()
    {
        gameObject.SetActive(true);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

}

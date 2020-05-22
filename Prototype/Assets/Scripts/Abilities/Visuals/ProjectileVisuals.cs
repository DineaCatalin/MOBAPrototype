using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbilityComponentManager), typeof(LocalParticleSystemManager))]
public class ProjectileVisuals : MonoBehaviour
{
    [SerializeField] bool useGameObject;
    [SerializeField] bool affectColliderOnActivation;
    [SerializeField] bool affectColliderOnDeactivation;
    [SerializeField] GameParticle particleOnDeath = GameParticle.NONE;

    LocalParticleSystemManager localParticleSystemManager;
    Collider2D projectileCollider;
    Tween tween;

    bool useTween;
    AbilityComponentManager abilityComponentManager;

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

        localParticleSystemManager = GetComponent<LocalParticleSystemManager>();

        tween = GetComponent<AbilitySpawnTween>();
        if (tween)
            useTween = true;
        else
            useTween = false;

        abilityComponentManager = GetComponent<AbilityComponentManager>();

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
        EnableAbilityComponents();
        isActiveOnScreen = true;
    }

    public void Deactivate()
    {
        isActiveOnScreen = false;

        deactivateAction.Invoke();
        SpawnDeathParticles();
        DisableAbilityComponents();
    }

    void ActivateAndTween()
    {
        ActivateParticleSystems();
        tween.Execute();
    }

    void SpawnDeathParticles()
    {
        if (particleOnDeath != GameParticle.NONE)
            ParticleEffectPool.Instance.SpawnParticle(particleOnDeath, transform.position);
    }

    void DeactivateColliderAndParticles()
    {
        Debug.LogError("ProjectileVisuals DeactivateColliderAndParticles");
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
        localParticleSystemManager.Play();
    }

    void DeactivateParticleSystems()
    {
        localParticleSystemManager.Stop();
    }

    void ActivateGameObject()
    {
        gameObject.SetActive(true);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    void EnableAbilityComponents()
    {
        abilityComponentManager.EnableAbilityComponents();
    }

    void DisableAbilityComponents()
    {
        abilityComponentManager.DisableAbilityComponents();
    }
}

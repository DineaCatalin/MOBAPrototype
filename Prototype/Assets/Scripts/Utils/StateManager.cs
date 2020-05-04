using UnityEngine;
using System.Collections;

// This will just activate/ deactivate the GO that is given 
public class StateManager : MonoBehaviour
{
    [SerializeField] GameObject managedObject;
    [SerializeField] Collider2D objectCollider;
    [SerializeField] LocalParticleSystemManager particles;
    [SerializeField] AbilityScaleTween tween;

    Coroutine deactivate;

    private void Awake()
    {
        objectCollider = GetComponentInChildren<Collider2D>();

        particles = GetComponentInChildren<LocalParticleSystemManager>();
        particles.enabled = false;

        tween = GetComponentInChildren<AbilityScaleTween>();
        tween.enabled = false;
    }

    public void Activate(float duration)
    {
        Debug.Log("StateManager Activate " + managedObject.name);
        Activate();

        if (deactivate != null)
            StopCoroutine(deactivate);

        deactivate = StartCoroutine(Deactivate(duration));
    }

    IEnumerator Deactivate(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("StateManager Activate " + managedObject.name);

        Deactivate();
    }

    public void SetLayer(int layer)
    {
        managedObject.layer = layer;
    }

    public void Deactivate()
    {
        //managedObject.SetActive(false);
        objectCollider.enabled = false;
        particles.DeactivateParticleSystems();
        tween.enabled = false;
    }

    void Activate()
    {
        objectCollider.enabled = true;
        tween.enabled = true;
        particles.ActivateParticleSystems();
    }

}

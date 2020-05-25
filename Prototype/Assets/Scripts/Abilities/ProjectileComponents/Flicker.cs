using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : AbilityComponent
{
    [SerializeField] float duration = 0.2f;

    ProjectileVisuals projectileVisuals;

    private void Awake()
    {
        projectileVisuals = GetComponent<ProjectileVisuals>();
    }

    // We use OnEnable because the GO that uses this component is used in an ObjectPool
    void OnEnable()
    {
        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Flicker Disable");
        projectileVisuals.DeactivateAndSpawnParticles();
    }
}

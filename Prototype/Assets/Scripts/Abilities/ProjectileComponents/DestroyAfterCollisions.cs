using UnityEngine;
using System.Collections;

// The wall will be destroyed if hit a number of times by abilities
public class DestroyAfterCollisions : AbilityComponent
{
    [SerializeField] int numCollisions;

    int health;

    ProjectileVisuals visuals;

    private void Awake()
    {
        visuals = GetComponent<ProjectileVisuals>();
        health = numCollisions;
    }

    private void Start()
    {
        EventManager.StartListening(GameEvent.EndRound, new System.Action(Destroy));
        EventManager.StartListening(GameEvent.PlanetStateAdvance, new System.Action(Destroy));
    }

    private void OnEnable()
    {
        health = numCollisions;
    }

    public void ApplyDamage()
    {
        health--;

        if (health <= 0)
            this.Destroy();
    }

    public void Destroy()
    {
        if(visuals.IsActiveOnScreen())
            visuals.DeactivateAndSpawnParticles();
    }
}

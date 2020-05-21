using UnityEngine;

public enum GameParticle
{
    BLINK,
    SPAWN,
    DEATH,
    FIRE_BALL_DESTROYED,
    ICE_BALL_DESTROYED,
    LIGHTNING_BALL_DESTROYED,
    FIRE_STORM_DESTROYED,
    PULL_DESTROYED,
    ICE_WALL_DESTROYED,
    SHIELD_DESTROYED,
    NONE
}

[System.Serializable]
public class ParticlePoolData
{
    public GameParticle particleName;
    public GameObject particleSystem;
    public int number;
}

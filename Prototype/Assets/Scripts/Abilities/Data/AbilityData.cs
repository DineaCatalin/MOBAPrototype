using System;
using System.Collections.Generic;

[Serializable]
public class AbilityDataList
{
    public List<AbilityData> dataList;
}

[Serializable]
public class AbilityData
{
    public AbilityDescription description;
    public AbilityStats stats;
}

[Serializable]
public class AbilityStats
{
    public int cooldown;     // Time till you can cast the ability again
    public int manaCost;     // How much mana will this ability cost?
    public int dotValue;     // Damage over time damage tick
    public int duration;     // Damage over time durration
    public int hpValue;      // If positive it's a heal if it's negative it's damage 
}

[Serializable]
public class AbilityDescription
{ 
    public string name;
    public string casterTeamName;
}

[Serializable]
public class ProjectileSpeedConfig
{
    public float FireballSpeed;
    public float FireStormSpeed;
    public float TornadoSpeed;
}

[Serializable]
public class AbilityCastRangeConfig
{
    public float TraceCastRange;
    public float IceWallCastRange;
    public float RootsCastRange;
    public float SpikesCastRange;
    public float ManaSphereCastRange;
    public float WaterRainCastRange;
    public float BlinkCastRange;
}
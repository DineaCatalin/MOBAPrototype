using UnityEngine;

public class AbilityData : MonoBehaviour
{
    public AbilityDescription description;
    public AbilityStats stats;
}

[System.Serializable]
public class AbilityStats
{
    public int cooldown;      // Time till you can cast the ability again
    public int manaCost;      // How much mana will this ability cost?
    public int dotDamage;     // Damage over time damage tick
    public int dotDuration;   // Damage over time durration
    public int hpDiff;        // If positive it's a heal if it's negative it's damage 
}

[System.Serializable]
public class AbilityDescription
{ 
    public string name;
    public string casterTeamName;
    //public bool isTrap;
}

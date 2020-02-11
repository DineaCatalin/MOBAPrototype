using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
    const int numAbilities = 8;
    string teamName;

    Ability[] abilities;
    Ability currentAbility;

    // Use this for initialization
    void Start()
    {

        //TODO: Load all abilities and set their casterTeamName to teamName and set the to be inactive

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();
    }

    // Set the current ability based on 
    void SetCurrentAbility(int index)
    {
        currentAbility = abilities[index];
    }

    public void CastAbility()
    {
        if(!currentAbility.isCharging)
        {
            currentAbility.Cast();
        }
        
    }

    void UpdateCooldowns()
    {
        for (int i = 0; i < numAbilities; i++)
        {
            if(abilities[i].isCharging)
                abilities[i].UpdateCooldown(Time.deltaTime);
        }
    }
}

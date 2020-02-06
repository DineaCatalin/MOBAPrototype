using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
    const int numAbilities = 8;
    string teamName;

    // The current selected ability
    Ability currentAbility;

    // For the moment make it visible in the inspector
    [SerializeField] Ability[] abilities;

    // Use this for initialization
    void Start()
    {
        // Get the name of the team we are in, we will need this for 
        teamName = GetComponent<Player>().teamName;
        //abilities = new Ability[numAbilities];

        //TODO: Load all abilities and set their casterTeamName to teamName


        // Test
        currentAbility = abilities[0];
        currentAbility.casterTeamName = teamName;

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Set the current ability based on 
    void SetCurrentAbility(int key)
    {
        currentAbility = abilities[key];
    }

    public void CastAbility(Vector3 position, Quaternion rotation)
    {
        currentAbility.Cast(position, rotation);
    }
}

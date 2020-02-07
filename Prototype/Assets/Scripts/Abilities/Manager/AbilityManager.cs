using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
    const int numAbilities = 8;
    string teamName;

    // The current selected ability
    GameObject currentAbility;

    // Index of the ability that is currently being used
    int currentAbilityIndex;

    // For the moment make it visible in the inspector
    [SerializeField] GameObject[] abilities;

    // Cache the current cooldowns 
    float[] cooldowns;

    // The actual values of the coolodowns that will be updated in real time
    float[] currentCooldowns;

    // Use this for initialization
    void Start()
    {


        //abilities = new Ability[numAbilities];
        //cooldowns = new int[numAbilities];

        //TODO: Load all abilities and set their casterTeamName to teamName and set the to be inactive


        // Test
        cooldowns = new float[1];
        currentCooldowns = new float[1];
        currentAbilityIndex = 0;
        currentAbility = abilities[0];
        var data = currentAbility.GetComponent<AbilityData>();
        data.description.casterTeamName = GetComponent<Player>().teamName;  //PLs not init here
        currentAbility.gameObject.SetActive(false);

        cooldowns[0] = data.stats.cooldown;
        currentCooldowns[0] = 0;
        Debug.Log("currentCooldown " + currentCooldowns[0] + " cooldowns " + cooldowns[0]);

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
        currentAbilityIndex = index;
    }

    public void CastAbility()
    {
        //if (!currentAbility.instaCast)
        //    currentAbility.transform.position = transform.position;
        if(currentCooldowns[currentAbilityIndex] <= 0 )
        {
            currentAbility.transform.position = transform.position;
            currentAbility.gameObject.SetActive(true);

            //Reset cooldown
            currentCooldowns[currentAbilityIndex] = cooldowns[currentAbilityIndex];
        }
        
    }

    void UpdateCooldowns()
    {
        float time = Time.deltaTime;

        //for (int i = 0; i < numAbilities; i++)
        //{
        //    currentCooldowns[i] -= time;
        //}

        currentCooldowns[0] -= time;
    }
}

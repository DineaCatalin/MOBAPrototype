using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponentManager : MonoBehaviour
{
    [SerializeField] bool deactivateComponentsOnAwake = true;

    AbilityComponent[] abilityComponents;

    // Start is called before the first frame update
    void Awake()
    {
        abilityComponents = GetComponents<AbilityComponent>();

        if (deactivateComponentsOnAwake)
            DisableAbilityComponents();
    }

    public void EnableAbilityComponents()
    {
        foreach (var abilityComponent in abilityComponents)
        {
            abilityComponent.enabled = true;
        }
    }

    public void DisableAbilityComponents()
    {
        foreach (var abilityComponent in abilityComponents)
        {
            abilityComponent.enabled = false;
        }
    }
}

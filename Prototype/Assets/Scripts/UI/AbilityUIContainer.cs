using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUIContainer : MonoBehaviour
{
    public static AbilityUIContainer Instance;

    public AbilityUI[] abilitieUIs;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    

    public AbilityUI GetUIForAbility(int index)
    {
        return abilitieUIs[index];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTest : MonoBehaviour
{
    PlanetManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<PlanetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            manager.Advance(Match.TEAM_1_ID);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            manager.Advance(Match.TEAM_2_ID);
        }
    }
}

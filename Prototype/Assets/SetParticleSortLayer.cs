using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParticleSortLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem pSyst = GetComponent<ParticleSystem>();

        //Change Foreground to the layer you want it to display on 
        //You could prob. make a public variable for this
        pSyst.GetComponent<Renderer>().sortingLayerName = "Foreground";
    }
}

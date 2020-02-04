using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
    Sprite hoverImage;
    new string name;


    // When the ability gets selected
    // Most of the times we will display a graphic to show the player the direction in which the ability will be shot 
    public virtual void OnSelect()
    {
        
    }

    public virtual void OnActivate()
    {

    }
}

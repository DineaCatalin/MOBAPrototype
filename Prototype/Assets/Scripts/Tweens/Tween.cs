using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tween : MonoBehaviour
{
    public bool useOnActivate = true;

    public abstract void Execute();
}

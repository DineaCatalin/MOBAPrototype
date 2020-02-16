using UnityEngine;
using System.Collections;

// The wall will be destroyed if hit a number of times by abilities
public class DestroyAfterCollisions : MonoBehaviour
{
    [SerializeField] int numCollisions;

    public void ApplyDamage()
    {
        numCollisions--;

        if (numCollisions <= 0)
            this.Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

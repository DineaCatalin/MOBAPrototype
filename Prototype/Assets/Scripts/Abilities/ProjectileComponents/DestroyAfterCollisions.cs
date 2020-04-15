using UnityEngine;
using System.Collections;

// The wall will be destroyed if hit a number of times by abilities
public class DestroyAfterCollisions : MonoBehaviour
{
    [SerializeField] int numCollisions;

    private void Start()
    {
        EventManager.StartListening(GameEvent.RoundEnd, new System.Action(Destroy));
        EventManager.StartListening(GameEvent.StartRedraft, new System.Action(Destroy));
    }

    public void ApplyDamage()
    {
        numCollisions--;

        if (numCollisions <= 0)
            this.Destroy();
    }

    public void Destroy()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}

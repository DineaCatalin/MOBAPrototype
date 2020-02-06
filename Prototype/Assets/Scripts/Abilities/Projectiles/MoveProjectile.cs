using UnityEngine;
using System.Collections;

public class MoveProjectile : MonoBehaviour
{
    // The postion towards which the ability will go to
    //public Vector2 target;

    // Speed at which the projectile will move to the target
    public float speed;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hreer");
        //Vector3 pos = transform.position + transform.forward * Time.deltaTime * speed;
        //transform.position += transform.forward * Time.deltaTime * speed;
        transform.Translate(transform.right * Time.deltaTime * speed);
    }
}

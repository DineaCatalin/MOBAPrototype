using UnityEngine;
using System.Collections;

public class MoveAbility : AbilityComponent
{
    // The postion towards which the ability will go to
    Vector2 moveDirection;

    // Speed at which the projectile will move to the target
    public float speed;

    Rigidbody2D thisRigidbody;

    void Start()
    {
        // Projectile will have "(Clone)" added to the name as it was instatiated after a template GO
        string correctName = name.Replace("(Clone)", "");
        speed = AbilityDataCache.GetProjectileSpeed(correctName);
        thisRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    transform.position = transform.position + moveDirection * speed * Time.deltaTime;
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = transform.position + moveDirection * speed * Time.deltaTime;
        Debug.Log("MoveAbility FixedUpdate moveDirection moveDirection * spee" + (moveDirection * speed));
        thisRigidbody.MovePosition((Vector2)transform.position + (moveDirection * speed * Time.fixedDeltaTime));
    }

    // Should be called right after the object is instantiated
    public void SetDirection(Vector3 target)
    {
        moveDirection = (target - transform.position);
        //moveDirection.z = 0;
        moveDirection.Normalize();
    }
}

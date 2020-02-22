using UnityEngine;
using System.Collections;

public class MoveAbility : MonoBehaviour
{
    // The postion towards which the ability will go to
    Vector3 moveDirection;

    // Speed at which the projectile will move to the target
    public float speed;

    void Start()
    {
        // Projectile will have "(Clone)" added to the name as it was instatiated after a template GO
        string correctName = name.Replace("(Clone)", "");
        speed = AbilityDataCache.GetProjectileSpeed(correctName);

        //moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        //moveDirection.z = 0;
        //moveDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + moveDirection * speed * Time.deltaTime;
    }

    // Should be called right after the object is instantiated
    public void SetDirection(Vector3 target)
    {
        moveDirection = (target - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();
    }
}

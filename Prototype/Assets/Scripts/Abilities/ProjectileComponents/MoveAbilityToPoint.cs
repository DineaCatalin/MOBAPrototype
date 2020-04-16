using UnityEngine;
using System.Collections;

public class MoveAbilityToPoint : MonoBehaviour
{
    // The postion towards which the ability will go to
    Vector3 moveDirection;

    Vector3 endPosition;

    // Speed at which the projectile will move to the target
    public float speed;

    void OnEnable()
    {
        // Projectile will have "(Clone)" added to the name as it was instatiated after a template GO
        string correctName = name.Replace("(Clone)", "");
        speed = AbilityDataCache.GetProjectileSpeed(correctName);
        Debug.Log("MoveAbilityToPoint speed for " + name + " is " + speed);

        moveDirection = Utils.Instance.GetMousePosition() - transform.position;
        moveDirection.z = 0;
        moveDirection.Normalize();

        endPosition = Utils.Instance.GetMousePosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + moveDirection * speed * Time.deltaTime;

        // Destroy the object if it is very very close to the target destination
        if (Vector2.Distance(transform.position, endPosition) < 0.1f)
            Destroy(this.gameObject);
    }
}

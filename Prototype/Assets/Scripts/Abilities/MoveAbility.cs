using UnityEngine;
using System.Collections;

public class MoveAbility : MonoBehaviour
{
    // The postion towards which the ability will go to
    Vector3 moveDirection;

    // Speed at which the projectile will move to the target
    public float speed;

    void OnEnable()
    {
        Debug.Log("MoveAbility::OnEnable");
        moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + moveDirection * speed * Time.deltaTime;
    }
}

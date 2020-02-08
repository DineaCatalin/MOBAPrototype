using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float speed;
    AbilityManager abilityManager;

    // Use this for initialization
    void Start()
    {
        speed = GetComponent<PlayerData>().speed;
        abilityManager = GetComponent<AbilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        //HandleRotation();
        Test();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))       // UP
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.S))       // DOWN
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.A))       // LEFT
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))       // RIGHT
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed, Space.World);
        }
    }

    void HandleRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 perpendicular = transform.position - mousePos;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
    }

    void Test()
    {
        if(Input.GetMouseButton(0))
        {
            abilityManager.CastAbility();
        }
    }

}

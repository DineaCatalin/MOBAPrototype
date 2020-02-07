using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    PlayerStats stats;
    AbilityManager abilityManager;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        abilityManager = GetComponent<AbilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        Test();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))       // UP
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.S))       // DOWN
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.A))       // LEFT
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))       // RIGHT
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * stats.speed, Space.World);
        }
    }

    void HandleRotation()
    {
        //Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 dir = Input.mousePosition - pos;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 45;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Struct that defines the stat of our player
    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = new PlayerStats
        {
            speed = 5f
        };
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        //Move Front/Back
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * stats.speed, Space.World);
        }
    }

    void HandleRotation()
    {
        Debug.Log("HandleRotation");

        //Vector3 mousePos = Input.mousePosition;
        //Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));
        //float z = Mathf.Atan2((screenPos.y - transform.position.y), (screenPos.x - transform.position.x)) * Mathf.Rad2Deg;
        //transform.Rotate(new Vector3(0, 0, z));

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}

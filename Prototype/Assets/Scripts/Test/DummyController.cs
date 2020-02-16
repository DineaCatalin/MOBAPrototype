using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour
{
    Vector3 movementIncrement;

    [SerializeField] float speed = 5f;

    private void Update()
    {
        movementIncrement = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))       // UP
        {
            movementIncrement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))       // DOWN
        {
            movementIncrement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))       // LEFT
        {
            movementIncrement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))       // RIGHT
        {
            movementIncrement += Vector3.right;
        }

        transform.Translate(movementIncrement * Time.deltaTime * speed, Space.World);
    }
}

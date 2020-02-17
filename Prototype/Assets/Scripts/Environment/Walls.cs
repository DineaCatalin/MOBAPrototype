using UnityEngine;
using System.Collections;

public class Walls : MonoBehaviour
{
    [SerializeField] Transform upWall;
    [SerializeField] Transform downWall;
    [SerializeField] Transform leftWall;
    [SerializeField] Transform rightWall;

    Vector3 screenDimension;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Position walls");
        screenDimension = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        upWall.position = new Vector3(0, screenDimension.y, 0);
        downWall.position = new Vector3(0, -screenDimension.y, 0);
        leftWall.position = new Vector3(-screenDimension.x, 0, 0);
        rightWall.position = new Vector3(screenDimension.x, 0, 0);
    }

}

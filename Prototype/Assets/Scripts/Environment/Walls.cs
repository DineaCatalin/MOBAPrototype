using UnityEngine;
using System.Collections;

public class Walls : MonoBehaviour
{
    [SerializeField] Transform upWall;
    [SerializeField] Transform downWall;
    [SerializeField] Transform leftWall;
    [SerializeField] Transform rightWall;

    [SerializeField] Transform abilityUpWall;
    [SerializeField] Transform abilityDownWall;
    [SerializeField] Transform abilityLeftWall;
    [SerializeField] Transform abilityRightWall;

    public int abilityWallPadding = 2;

    Vector3 environmentSize;

    // Use this for initialization
    void Start()
    {
        Invoke("SetWallPositions", 1f);
    }

    void SetWallPositions()
    {
        environmentSize = EnvironmentManager.Instance.environmentSize;
        upWall.position = new Vector3(0, environmentSize.y, 0);
        downWall.position = new Vector3(0, -environmentSize.y, 0);
        leftWall.position = new Vector3(-environmentSize.x, 0, 0);
        rightWall.position = new Vector3(environmentSize.x, 0, 0);

        abilityUpWall.position = new Vector3(0, environmentSize.y + abilityWallPadding, 0);
        abilityDownWall.position = new Vector3(0, -environmentSize.y - abilityWallPadding, 0);
        abilityLeftWall.position = new Vector3(-environmentSize.x - abilityWallPadding, 0, 0);
        abilityRightWall.position = new Vector3(environmentSize.x + abilityWallPadding, 0, 0);

        Debug.Log("SYNC_ENV_POS Position walls: Up " + upWall.position + " Down " + downWall.position + " Left " + leftWall.position + " Right " + rightWall.position);

    }
}

using UnityEngine;
using Photon.Pun;

public enum AbilityInputKey
{
    // Ability1 Will be left  click 
    // Ability2 Will be right click 
    AttackAbility = KeyCode.Mouse0,
    DefenseAbility = KeyCode.Mouse1,
    MobilityAbility = KeyCode.Space,
    Ability1 = KeyCode.Q,
    Ability2 = KeyCode.E,
    Ability3 = KeyCode.X,
    Ability4 = KeyCode.C,
    AbilityAlias1 = KeyCode.Alpha1,
    AbilityAlias2 = KeyCode.Alpha2,
    AbilityAlias3 = KeyCode.Alpha3,
    AbilityAlias4 = KeyCode.Alpha4,
    AbilityManaCharge = KeyCode.LeftShift    
}

public class PlayerController : MonoBehaviour
{
    public Player player;

    AbilityManager abilityManager;
    Transform playerTransform;
    Rigidbody2D playerRigidbody;

    public static bool isLocked;

    // We will disable the movement function when this is true
    public static bool isRooted;
    
    PlayerData stats;

    Vector3 movementIncrement;

    PhotonView photonView;

    bool charging;

    float AngleRad;
    float AngleDeg;

    // Use this for initialization
    void Start()
    {
        isLocked = true;

        stats = player.GetStats();
        charging = false;

        abilityManager = player.GetComponent<AbilityManager>();
        playerTransform = player.transform;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    bool abilityWasCast;

    // Update is called once per frame
    void Update()
    {
        if(!isLocked)
        {
            HandleRotationTransform();
            HandleManaCharge();
            //HandleAbilityCasting();   // As all abilities are instant now this will just create a bug 
            HandleAbilitySelection();   // and this will trigger all the abilities when they are selected
        }
    }

    private void FixedUpdate()
    {
        if (!isLocked)
        {
            HandleMovementRigidbody();
            //HandleRotationRigidBody();
        }
    }

    Vector2 direction;

    void HandleMovementRigidbody()
    {
        if (!isRooted)
        {
            direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            playerRigidbody.MovePosition((Vector2)playerTransform.position + (direction * stats.speed * Time.deltaTime));
        }
    }

    void HandleMovementTransform()
    {
        movementIncrement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))       // UP
        {
            movementIncrement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))       // DOWN
        {
            movementIncrement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))       // LEFT
        {
            movementIncrement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))       // RIGHT
        {
            movementIncrement += Vector3.right;
        }

        playerTransform.Translate(movementIncrement * Time.deltaTime * stats.speed, Space.World);
    }

    void HandleRotationRigidBody()
    {
        AngleRad = Mathf.Atan2(Input.mousePosition.y - playerTransform.position.y, Input.mousePosition.x - playerTransform.position.x);
        AngleDeg = (180 / Mathf.PI) * AngleRad;
        playerRigidbody.rotation = AngleDeg;
    }

    void HandleRotationTransform()
    {
        playerTransform.rotation = Quaternion.LookRotation(Vector3.forward, playerTransform.position - Utils.Instance.GetMousePosition());
    }

    bool HandleAbilityCasting()
    {
        if(Input.GetMouseButton(0))
        {
            if (abilityManager.NoAbilitySelected() || abilityManager.IsCurrentAbilityCharging())
            {
                // Maybe show a UI text saying the current ability is still charging
                return false;
            }

            abilityManager.CastAbility();
            abilityWasCast = true;
            return true;
        }

        abilityWasCast = false;
        return false;
    }

    bool HandleManaCharge()
    {
        if(Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityManaCharge))
        {
            if(!charging)
            {
                player.StartManaCharge();
                charging = true;
            }

            return true;
        }
        else if(Input.GetKey((KeyCode)AbilityInputKey.AbilityManaCharge))
        {
            // Plauyer 
            return true;
        }
        else if(Input.GetKeyUp((KeyCode)AbilityInputKey.AbilityManaCharge))
        {
            player.StopManaCharge();
            charging = false;
        }

        return false;
    }

    void HandleAbilitySelection()
    {
        if (Input.GetMouseButton(0) && !abilityWasCast)
        {
            SwitchSelectedAbility(1);
            //return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            SwitchSelectedAbility(2);
            //return;
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.MobilityAbility))
        {
            SwitchSelectedAbility(3);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability1) ||
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias1))
        {
            SwitchSelectedAbility(4);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability2) ||
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias2))
        {
            SwitchSelectedAbility(5);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability3) ||
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias3))
        {
            SwitchSelectedAbility(6);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability4) || 
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias4))
        {
            SwitchSelectedAbility(7);
        }
    }

    void SwitchSelectedAbility(int index)
    {
        // We do this so that the number of the ability becomes the position in the array
        index--;
        abilityManager.SetCurrentAbility(index);  
    }
}

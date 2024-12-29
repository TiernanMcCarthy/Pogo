using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PogoController : MonoBehaviour
{
    private PlayerControls inputActions;

    private Rigidbody rb;

    [Header("Transforms")]
    [SerializeField] private Transform activeCamera;
    [SerializeField] private Transform bottomOfPogo;
    [SerializeField] private Transform centreOfMass;
    [SerializeField] private Transform springTransform;
    [SerializeField] private Transform triggerTestTransform;

    [Header("Character Movement Variables")]
    [SerializeField] private float rotateForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxAngularVelocity;
    [SerializeField] private Vector3 artificalGravityDirection;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float upRightFactor = 0.3f; //Factor of momentum that is used to stay upright

    //Drag is different whilst on the ground and whilst the spring is readying for a jump
    //Higher drag makes the pogo more stable and allows for the player to land and jump more smoothly in quick succession
    [SerializeField] private float normalDrag;
    [SerializeField] private float airTimeDrag;
    [SerializeField] private float jumpingDrag;
    [SerializeField] private float angularDrag = 0.05f;
    [SerializeField] private float groundedAngularDragMultiplier = 3.0f;
    private bool bouncing = false;

    [Header("Spring Properties")]
    [SerializeField] private float highestJump;
    [SerializeField] private float maxJumpTime;

    //The spring is physically scaled on the y axis to change the pogo height and emulate the spring change
    [SerializeField] private float springHeight;
    [SerializeField] private float minSpringSize;

    [SerializeField] private float timeBetweenJumps;

    #region Local Private Variables

    //States
    private bool jumping; //Build up to spring Jump
    private bool springReady = true; //Specifies that the spring is ready to be used to jump
    private bool significantMovement = false;
    //Jumping Variables
    private float jumpTime;
    private float lastGroundTime;
    private float jumpLerp = 0; //Manages spring size by evaluating jumptime against the maxJumpTime variable, as a ratio of 0-1


    [SerializeField]
    private List<Modifier> modifiers = new List<Modifier>();

    public UnityEvent OnJump;

    private float jumpModifier = 1;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Manage Controls
        inputActions = new PlayerControls();
        inputActions.Enable();
        
        //Manage Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.useGravity = false;
        //Set Gravity
        artificalGravityDirection = Physics.gravity;

        modifiers= new List<Modifier>();
    }

    public void SetGravity(Vector3 gravityDir)
    {
        artificalGravityDirection = gravityDir;
    }

    public Vector3 GetGravity()
    {
        return artificalGravityDirection;
    }

    private float GetGroundAngleRelativeToGravity()
    {
        RaycastHit hit;
        float slopeAngle = 0;
        if(Physics.Raycast(springTransform.position, springTransform.up * -1, out hit, springHeight * 1.2f,~0, QueryTriggerInteraction.Ignore))
        {
           slopeAngle = Vector3.Angle(hit.normal, -artificalGravityDirection);
        }
        return slopeAngle;
    }

    private void RotatePogo()
    {
        // Get input directions
        Vector2 inputs = new Vector2(inputActions.Player.Horizontal.ReadValue<float>(), inputActions.Player.Vertical.ReadValue<float>());

        if(inputs.magnitude>1.6f)
        {
            significantMovement = true;
        }
        else
        {
            significantMovement = false;
        }
        // Calculate the gravity direction (assuming a custom gravity vector is used)
        Vector3 gravityDirection = artificalGravityDirection.normalized;

        // Calculate the "up" direction for the player, which is opposite to gravity
        Vector3 upAxis = -gravityDirection;

        // Calculate the camera's forward and right directions
        Vector3 cameraForward = activeCamera.forward;
        Vector3 cameraRight = activeCamera.right;

        // Adjust the forward direction to align with the camera's "up"
        Vector3 adjustedForward = Vector3.Cross(cameraRight, upAxis).normalized;

        // Adjust the right direction to ensure it's orthogonal to gravity
        Vector3 adjustedRight = Vector3.Cross(upAxis, adjustedForward).normalized;

        // Calculate the desired movement direction based on inputs
        Vector3 moveDirection = adjustedForward * inputs.y + adjustedRight * inputs.x;

        // Calculate torque to rotate the object around an axis perpendicular to gravity
        Vector3 torque = Vector3.Cross(upAxis, moveDirection) * rotateForce;

        // Apply torque to rotate the object
        rb.AddTorque(torque, ForceMode.VelocityChange);

        // Set the center of mass
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
    }

    //Store "Landing" force and use this as a multiplier for next jump for a few seconds before it fades?
    private void Jump(float jumpTime)
    {
        Vector3 dir = transform.up * jumpForce * Mathf.Clamp(jumpTime,0.3f,maxJumpTime)*jumpModifier;

        if(dir.magnitude>highestJump)
        {
            dir = transform.up * highestJump;
        }
        rb.drag = airTimeDrag;
        rb.AddForce(dir, ForceMode.Acceleration);
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
        jumping = false;

        StartCoroutine(ResetSpring());
        OnJump.Invoke();
    }

    //The Pogo is stabilised by trending towards the "gravity" direction at this moment, can be and should be affected by world gravity or an area's custom gravity
    private void Stabilise()
    {
        if (!significantMovement && Time.time-jumpTime>0.7f || CheckIfGrounded() && !jumping)
        {
            // Apply torque to rotate the object towards where "gravity" is at this moment
            Vector3 torque = upRightFactor * rotateForce * -artificalGravityDirection;

            rb.AddTorque(torque, ForceMode.Force);
        }
        else if(jumping)
        {
            Vector3 torque = transform.up * rotateForce * upRightFactor*0.23f;

            rb.AddTorque(torque, ForceMode.Force);
        }
        else
        {
           // Vector3 torque = -artificalGravityDirection * rotateForce * upRightFactor*0.23f;
           // rb.AddTorque(torque, ForceMode.Force);
        }
    }

    private void ResetJump() 
    {
        jumping = false;
        rb.drag = normalDrag;
    }

    private IEnumerator ResetSpring(float startPoint=0)
    {
        float yScale = 0;
        float resetTime = Time.time;
        while(true)
        {
            yScale= startPoint+(Time.time- resetTime)/timeBetweenJumps;
            //Manage Spring Scale
            springTransform.localScale = new Vector3(springTransform.localScale.x, yScale*springHeight, springTransform.localScale.z);
            yield return  new WaitForFixedUpdate();
            if(yScale>=1)
            {
                break;
            }
        }

        springReady = true;
    }

    private bool CheckIfGrounded()
    {
        RaycastHit hit;
        // Cast ray down to detect the terrain or surface below the player
        if (Physics.Raycast(springTransform.position, springTransform.up*-1, out hit, springHeight*1.45f,~0,QueryTriggerInteraction.Ignore))
        {
            lastGroundTime = Time.time;
            if (!jumping)
            {
                rb.drag = normalDrag;
            }
            return true;
        }

        rb.drag = airTimeDrag;

        return false;
    }
    private void NewPogoJump()
    {
        if(inputActions.Player.Jump.IsPressed() && jumping==false && springReady && CheckIfGrounded())
        {
            jumping = true;
            rb.drag = jumpingDrag;
            jumpTime = Time.time;
            springReady = false;

            //Give a bit more grip if we detect a wall of at least 70 degrees against gravity
            if(GetGroundAngleRelativeToGravity()>70)
            {
                rb.drag = rb.drag * 1.1f;
            }
        }

        if(jumping)
        {
            CheckIfGrounded();
            if(Time.time-lastGroundTime>coyoteTime)
            {
                ResetJump();
                StartCoroutine(ResetSpring(jumpLerp));
            }
            //Manage Spring Size
            jumpLerp = (Time.time - jumpTime) / maxJumpTime;

            //Manage Spring Scale
            springTransform.localScale = new Vector3(springTransform.localScale.x, Mathf.Clamp((1-jumpLerp) * springHeight,0.001f,springHeight), springTransform.localScale.z);

            if(Time.time-jumpTime>maxJumpTime)
            {
                ResetJump();
                Jump(maxJumpTime);
                return;
            }

            if(!inputActions.Player.Jump.IsPressed())
            {
                ResetJump();
                Jump(Time.time - jumpTime);
                StartCoroutine(ResetSpring(jumpLerp));
                return;
            }
        }
    }

    private Collider[] TestTriggerBox()
    {
        return Physics.OverlapBox(triggerTestTransform.transform.position, new Vector3(0.05f, 0.065f, 0.05f), triggerTestTransform.transform.rotation);
    }
    /// <summary>
    /// Iterate through all modifiers and add their effects, these will be reflected in their respective functions
    /// </summary>
    private void CheckModifiers()
    {
        modifiers.Clear();
        Collider[] collisions=TestTriggerBox();

        foreach(Collider coll in collisions)
        {
            if (coll.CompareTag("Modifier"))
            {
                modifiers.Add(coll.GetComponent<Modifier>());
            }
        }
        jumpModifier = 1;
        foreach(var modifier in modifiers)
        {
            jumpModifier += modifier.jumpModifier;
        }
    }

    //The player's rigid body gravity is disabled by default, an ARTIFICAL direction is used, this allows for stage gravity to change
    //Triggers or the level gravity should change based upon design
    private void ApplyGravity()
    {
        rb.AddForce(artificalGravityDirection);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckIfGrounded();
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
        CheckModifiers();
        RotatePogo();
        NewPogoJump();
        Stabilise();
        ApplyGravity();
    }

    public void ResetPlayerVelocity()
    {
        rb.velocity= Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider col)
    {
        /*
        if(col.gameObject.CompareTag("Modifier"))
        {
            foreach (Modifier m in modifiers) //Remove if found
            {
                if (m.gameObject == col.gameObject)
                {
                    return;
                }
            }
            Modifier mod =col.gameObject.GetComponent<Modifier>();
            modifiers.Add(mod);
        }*/

    }

    private void OnTriggerExit(Collider other)
    {
        //Check for modifiers
        /*

        foreach(Modifier m in modifiers) //Remove if found
        {
            if(m.gameObject==other.gameObject)
            {
                modifiers.Remove(m);
                break;
            }
        }*/
    }
}

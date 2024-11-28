using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Character Movement Variables")]
    [SerializeField] private float rotateForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxAngularVelocity;
    [SerializeField] private Vector3 artificalGravityDirection;

    //Drag is different whilst on the ground and whilst the spring is readying for a jump
    //Higher drag makes the pogo more stable and allows for the player to land and jump more smoothly in quick succession
    [SerializeField] private float normalDrag;
    [SerializeField] private float jumpingDrag;

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

    private float jumpLerp = 0; //Manages spring size by evaluating jumptime against the maxJumpTime variable, as a ratio of 0-1



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
    }

    public void SetGravity(Vector3 gravityDir)
    {
        artificalGravityDirection = gravityDir;
    }

    public Vector3 GetGravity()
    {
        return artificalGravityDirection;
    }
    /* WORKS ISH
    private void RotatePogo()
    {
        // Get input dirs
        Vector2 inputs = new Vector2(inputActions.Player.Horizontal.ReadValue<float>(),inputActions.Player.Vertical.ReadValue<float>());

        // Calculate the desired movement direction relative to the camera

        Vector3 forward = activeCamera.forward;
        Vector3 right = activeCamera.right;
        Vector3 up = activeCamera.up;

        Vector3 gravityDirection = artificalGravityDirection.normalized;
        // Ensure the player moves without any respect to where the camera is vertically
        forward.y = 0;
        right.y = 0;
        up.y = 0;

        forward.Normalize();
        right.Normalize();

        // Calculate the movement direction based on input
        Vector3 moveDirection = forward * inputs.y + right * inputs.x;
        
        // Apply torque to rotate the object around the opposite of the gravity normal
        Vector3 torque = Vector3.Cross(gravityDirection, moveDirection) * rotateForce;

        // Rotate the pogo with torque, instant velocity means the control is responsive
        rb.AddTorque(torque, ForceMode.VelocityChange);

        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
    }
    ORIGINAL

     private void RotatePogo()
{
    // Get input dirs
    Vector2 inputs = new Vector2(inputActions.Player.Horizontal.ReadValue<float>(),inputActions.Player.Vertical.ReadValue<float>());

    // Calculate the desired movement direction relative to the camera

    Vector3 forward = activeCamera.forward;  
    Vector3 right = activeCamera.right;  

    // Ensure the player moves without any respect to where the camera is vertically
    forward.y = 0;
    right.y = 0;

    forward.Normalize();
    right.Normalize();

    // Calculate the movement direction based on input
    Vector3 moveDirection = forward * inputs.y + right * inputs.x;
    
    // Apply torque to rotate the object around its up-axis (Y-axis)
    Vector3 torque = Vector3.Cross(Vector3.up, moveDirection) * rotateForce;

    // Rotate the pogo with torque, instant velocity means the control is responsive
    rb.AddTorque(torque, ForceMode.VelocityChange);

    rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
}*/ 


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
        Vector3 dir = transform.up * jumpForce * Mathf.Clamp(jumpTime,0.3f,maxJumpTime);

        if(dir.magnitude>highestJump)
        {
            dir = transform.up * highestJump;
        }

        rb.AddForce(dir, ForceMode.Acceleration);
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);

        StartCoroutine(ResetSpring());
    }

    //The Pogo is stabilised by trending towards the "gravity" direction at this moment, can be and should be affected by world gravity or an area's custom gravity
    private void Stabilise()
    {
        if (!significantMovement && Time.time-jumpTime>0.7f || CheckIfGrounded() && !jumping)
        {
            // Apply torque to rotate the object towards where "gravity" is at this moment
            Vector3 torque = -artificalGravityDirection * rotateForce * 0.45f;

            rb.AddTorque(torque, ForceMode.Force);
        }
        else if(jumping)
        {
            Vector3 torque = -artificalGravityDirection * rotateForce * 0.15f;

            rb.AddTorque(torque, ForceMode.Force);
        }
        else
        {
            Vector3 torque = -artificalGravityDirection * rotateForce * 0.15f;
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
        if (Physics.Raycast(springTransform.position, springTransform.up*-1, out hit, springHeight*1.2f))
        {
            return true;
        }

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
        }

        if(jumping)
        {
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

    //The player's rigid body gravity is disabled by default, an ARTIFICAL direction is used, this allows for stage gravity to change
    //Triggers or the level gravity should change based upon design
    private void ApplyGravity()
    {
        rb.AddForce(artificalGravityDirection);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
        RotatePogo();
        NewPogoJump();
        Stabilise();
        ApplyGravity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody == null)
        {
            
        }

    }
}

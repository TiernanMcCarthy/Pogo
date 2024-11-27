using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PogoController : MonoBehaviour
{
    private PlayerControls inputActions;

    private Rigidbody rb;

    [SerializeField] private Transform activeCamera;

    [SerializeField] private Transform bottomOfPogo;

    [SerializeField] private float rotateForce;

    [SerializeField] private float jumpForce;

    [SerializeField] private float maxAngularVelocity;

    [Header("Pogo Spring Options")]
    [SerializeField] private float highestJump;
    [SerializeField] private AnimationCurve springCollapseCurve;
    [SerializeField] private float floatHeight;
    [SerializeField] private float defaultSpringStrength;
    [SerializeField] private float springDamper;
    [SerializeField] private float springStrength;

    [SerializeField] private Transform centreOfMass;

    private bool bouncing = false;

    [Header("Spring Properties")]
    [SerializeField] private Transform springTransform;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float springHeight;
    [SerializeField] private float minSpringSize;
    [SerializeField] private float springJumpForce;
    [SerializeField] private float timeBetweenJumps;
    [SerializeField] private float normalDrag;
    [SerializeField] private float jumpingDrag;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Enable();
        
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = maxAngularVelocity;
    }

    /*

    private IEnumerator WaitForNextJump()
    {
        yield return new WaitForSeconds(0.8f);
        jumping = false;
    }
    private void ManagePogoSpring()
    {
        if (bouncing)
        {
            springStrength *= 0.96f;
        }
        RaycastHit hit;

        //Physics.BoxCast()
        // Cast ray down to detect the terrain or surface below the player
        if (Physics.Raycast(bottomOfPogo.position, bottomOfPogo.transform.up*-1, out hit, floatHeight) && !jumping)
        {
            if(bouncing==false)
            {
                bouncing = true;
                jumpTime = Time.time;
                springStrength= defaultSpringStrength;
            }
            Vector3 velocity = rb.velocity;

            Vector3 rayDir = transform.TransformDirection(bottomOfPogo.transform.up * -1);

            Vector3 otherVel = Vector3.zero;

            Rigidbody hitBody = hit.rigidbody;

            if(hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, velocity);
            float otherDirVel= Vector3.Dot(rayDir, otherVel);

            float relativeVelocity= rayDirVel - otherDirVel;

            float x= hit.distance - floatHeight;

            float springForce = (x*springStrength)- (relativeVelocity*springDamper);

            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.yellow);

            rb.AddForce(rayDir * springForce);
            rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
            if (hitBody !=null)
            {
                hitBody.AddForceAtPositionLocal(rayDir * -springForce, hit.point);
                //hitBody.AddForceAtPosition(rayDir * -springForce, hit.point);
            }
            if(Mathf.Abs(x)>floatHeight*0.85f)
            {
                bouncing = false;

                float multiplier=springCollapseCurve.Evaluate(Time.time-jumpTime);

                Jump(multiplier);

                jumping = true;

                StartCoroutine(WaitForNextJump());
                Debug.Log("BOUNCE");
            }
        }
        else if(Time.time-jumpTime>1.4f)
        {
            bouncing=false;
        }
    }*/
    private void RotatePogo()
    {

        // Get input dirs
        Vector2 inputs = new Vector2(inputActions.Player.Horizontal.ReadValue<float>(),inputActions.Player.Vertical.ReadValue<float>());

        // Calculate the desired movement direction relative to the camera
        Vector3 forward = activeCamera.forward;  // Camera's forward direction
        Vector3 right = activeCamera.right;  // Camera's right direction

        // Flatten the forward vector to ignore any vertical component
        forward.y = 0;
        right.y = 0;

        // Normalize the vectors to ensure consistent movement speed
        forward.Normalize();
        right.Normalize();

        
        // Calculate the movement direction based on input
        Vector3 moveDirection = forward * inputs.y + right * inputs.x;

        
        // Apply torque to rotate the object around its up-axis (Y-axis)
        Vector3 torque = Vector3.Cross(Vector3.up, moveDirection) * rotateForce;

        // Apply the torque to the Rigidbody
        rb.AddTorque(torque, ForceMode.VelocityChange);

        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
    }
    //Store "Landing" force and use this as a multiplier for next jump for a few seconds before it fades
    public void Jump(float jumpTime)
    {
        Vector3 dir = transform.up * jumpForce * Mathf.Clamp(jumpTime,0.3f,maxJumpTime);

        if(dir.magnitude>highestJump)
        {
            dir = transform.up * highestJump;
        }
        rb.AddForce(dir, ForceMode.Acceleration);
        //rb.AddForceAtPositionLocal(dir, transform.InverseTransformPoint(bottomOfPogo.transform.position));
        //rb.AddForceAtPosition(dir,bottomOfPogo.transform.position);
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);

        StartCoroutine(ResetSpring());
    }

    void Stabilise()
    {
        if(jumping)
        {
           // return;
        }

        
        // Apply torque to rotate the object around its up-axis (Y-axis)
        Vector3 torque = Vector3.up * rotateForce*0.35f;

        // Apply the torque to the Rigidbody
        //rb.AddTorque(torque, ForceMode.VelocityChange);
        // Vector3.RotateTowards()

        rb.AddTorque(torque, ForceMode.Force);
        //rb.MoveRotation(Quaternion.Slerp(rb.transform.rotation, Quaternion.Euler(Vector3.up), 0.3f));
    }

    private bool jumping;

    private bool springReady = true;

    private float jumpTime;
    public float jumpLerp = 0;

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
        if (Physics.Raycast(springTransform.position, springTransform.up*-1, out hit, springHeight))
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

            //Minus fluff to prevent /0
            //jumpLerp= (maxJumpTime/ (Time.time - jumpTime - 0.001f) ) *-1;

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

    //Unusued
    private void PogoSpring()
    {
        RaycastHit hit;

        //Physics.BoxCast()
        // Cast ray down to detect the terrain or surface below the player
        if (Physics.Raycast(bottomOfPogo.position, Vector3.down, out hit, floatHeight) && !jumping)
        {
            Debug.Log(hit.collider.gameObject.name);
            Vector3 velocity = rb.velocity;

            Vector3 rayDir = transform.TransformDirection(Vector3.down);

            Vector3 otherVel = Vector3.zero;

            Rigidbody hitBody = hit.rigidbody;

            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, velocity);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relativeVelocity = rayDirVel - otherDirVel;

            float x = hit.distance - floatHeight;

            float springForce = (x * springStrength) - (relativeVelocity * springDamper);

            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.yellow);

            rb.AddForce(rayDir * springForce);
            rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);
            if (hitBody != null)
            {
                hitBody.AddForceAtPositionLocal(rayDir * -springForce, hit.point);
                //hitBody.AddForceAtPosition(rayDir * -springForce, hit.point);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.centerOfMass = rb.transform.InverseTransformPoint(centreOfMass.transform.position);

        RotatePogo();
        NewPogoJump();
        //ManagePogoSpring();
        Stabilise();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody == null)
        {
            
        }

    }
}

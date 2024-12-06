using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMagnet : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField]
    private float magnetForce;
    [SerializeField]
    private float magnetDistance;
    [SerializeField]
    private float launchForce;
    [SerializeField]
    private float maxGrabSpeed;

    [Header("Transforms")]
    [SerializeField]
    private Transform magnetPosition;
    [SerializeField]
    private BoxCollider magnetCollider;
    [SerializeField]
    private Transform grabPosition;

    [SerializeField]
    private Rigidbody rbPogo;
    [SerializeField]
    private PogoController pogo;


    private PlayerControls inputActions;


    private float magnetPhysicalDistance = 0;

    [SerializeField]
    private Rigidbody grabbedObject;

    private float grabForce = 0;

    private bool tightGrab;

    private Vector3 objLastPos;

    private Rigidbody lastGrabbedObject=null;
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Enable();
        magnetCollider.transform.localPosition=magnetPosition.localPosition + magnetPosition.up*magnetDistance/2;
        magnetCollider.transform.localScale = new Vector3(magnetCollider.transform.localScale.x, magnetDistance, magnetCollider.transform.localScale.z);
        magnetPhysicalDistance = magnetCollider.transform.localScale.y;

        pogo.OnJump.AddListener(Jumping);
    }

    private void ManageInputs()
    {
        grabForce = inputActions.Player.Trigger.ReadValue<float>();
        
    }
    // Update is called once per frame
    void Update()
    {
        ManageInputs();
    }

    private void Jumping()
    {
        if(grabbedObject!=null)
        {
            grabbedObject.velocity += rbPogo.velocity;
        }
    }
    private void MagnetiseObject()
    {
        if(grabbedObject != null) //No Grabbed Object
        {
            //nf "next frame position" for these forces
            Vector3 nfGrabPosition = grabPosition.position + rbPogo.velocity * Time.fixedDeltaTime;
            if (Vector3.Distance(grabbedObject.transform.position, grabPosition.position) > 0.18f) //At distance we want a lighter force to pull the object to the rest point
            {
                grabbedObject.AddForce(magnetForce * grabForce * (nfGrabPosition - grabbedObject.transform.position), ForceMode.VelocityChange);
            }
            else if(Vector3.Distance(grabbedObject.position, grabPosition.position)<0.19f) //At these close distances, the object should lock a bit more, this allows us to really mess with the object
            {
                grabbedObject.AddForce(magnetForce * grabForce * (nfGrabPosition - grabbedObject.transform.position), ForceMode.Impulse);
                grabbedObject.angularDrag = 3;
            }
            grabbedObject.velocity=grabbedObject.velocity.normalized*Mathf.Clamp(grabbedObject.velocity.magnitude, 0, 2.5f);
            grabbedObject.drag = 3;
        }
    }
    private void CheckGrabbedObject()
    {
        Collider[] hits=Physics.OverlapBox(magnetCollider.transform.position, magnetCollider.transform.localScale*0.5f, magnetCollider.transform.rotation);

        Rigidbody closestRigidbody = null;
        float closestDistance = 999;
        if (grabForce > 0)
        {
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.CompareTag("Magnetic") && hit.attachedRigidbody != null)
                {
                    float distance = Vector3.Distance(grabPosition.position, hit.attachedRigidbody.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestRigidbody = hit.attachedRigidbody;
                    }
                }
            }
        }
        if (closestRigidbody != null)
        {
            grabbedObject = closestRigidbody;
        }
        else if(grabbedObject != null)
        {
            grabbedObject.drag = 0;
            grabbedObject = null;
        }

    }
    private void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            grabbedObject.angularDrag = 0.05f;
            objLastPos = grabbedObject.transform.position;
        }
        CheckGrabbedObject();
        MagnetiseObject();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(magnetPosition.position, magnetPosition.position + magnetPosition.up * magnetCollider.transform.localScale.y);
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RigidBodyExtensions
{
    public static void AddForceAtPositionLocal(this Rigidbody body, Vector3 force, Vector3 position,
            ForceMode forceMode = ForceMode.Force)
    {
        Quaternion rotation = body.transform.rotation;
        body.AddForce(rotation * force, forceMode);
        body.AddTorque(rotation * Vector3.Cross(position - body.centerOfMass, force), forceMode);
    }

}

public class PogoManual : MonoBehaviour
{

    private PlayerControls inputActions;

    private Rigidbody rb;

    [SerializeField] private Transform activeCamera;

    [SerializeField] private Transform bottomOfPogo;

    [SerializeField] private float rotateForce;

    [SerializeField] private float jumpForce;

    [Header("Pogo Spring Options")]
    [SerializeField] private float highestJump;
    [SerializeField] private AnimationCurve springCollapseCurve;
    [SerializeField] private float floatHeight;
    [SerializeField] private float defaultSpringStrength;
    [SerializeField] private float springDamper;
    [SerializeField] private float springStrength;

    [SerializeField] private Transform centreOfMass;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

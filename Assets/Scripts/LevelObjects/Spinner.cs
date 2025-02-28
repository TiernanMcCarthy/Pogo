using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    [SerializeField] private Vector3 rotationAxis;

    [SerializeField] private bool useRigidBody = false;

    [SerializeField] private bool staticPos;

    private Vector3 rotations;

    private Rigidbody rb;

    private Vector3 holdPosition;
    // Start is called before the first frame update
    void Start()
    {
        rotations = transform.rotation.eulerAngles;
        if(useRigidBody)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        holdPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (useRigidBody)
        {
            rb.AddTorque(rotationAxis * rotateSpeed);
        }
        else
        {
            rotations += rotationAxis.normalized * rotateSpeed;
            transform.localRotation = Quaternion.Euler(rotations);
        }

        if(staticPos)
        {
            transform.position = holdPosition;
        }
       // Vector3 localrot = transform.localRotation.eulerAngles;
       // localrot.x *= rotationAxis.x; localrot.y *= rotationAxis.y; localrot.z *= rotationAxis.z;
       // transform.localRotation=Quaternion.Euler(localrot);
    }
}

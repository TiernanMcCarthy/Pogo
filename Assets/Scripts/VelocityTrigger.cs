using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrigger : MonoBehaviour
{
    [SerializeField] private CopyVelocity target;
    [SerializeField] private Vector3 directionOffset;
    [SerializeField] private float offsetDistance = 20;
    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.gameObject == PlayerManagement.player.gameObject)
            {
                target.ActivateRigidBody();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        target.transform.position = transform.position + transform.up * -1 * offsetDistance;
        target.transform.position += directionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

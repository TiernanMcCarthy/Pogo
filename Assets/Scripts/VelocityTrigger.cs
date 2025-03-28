using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrigger : MonoBehaviour
{
    [SerializeField] private CopyVelocity target;
    [SerializeField] private Vector3 directionOffset;
    [SerializeField] private float offsetDistance = 20;

    [SerializeField] private bool lerpPos;

    private bool isActive = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.gameObject == PlayerManagement.player.gameObject)
            {
                target.ActivateRigidBody();
                isActive = false;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        target.transform.position= new Vector3(
            target.transform.position.x,transform.position.y-offsetDistance, 
            target.transform.position.z);
        //target.transform.position = transform.position + transform.up * -1 * offsetDistance;
        //target.transform.position += directionOffset;
    }

    public void MoveBeneathPlayer()
    {
        isActive = true;
    }



    // Update is called once per frame
    void Update()
    {
        if(lerpPos && isActive)
        {
            Vector3 playerPos = PlayerManagement.player.transform.position;
            target.transform.position= new Vector3(playerPos.x, target.transform.position.y, playerPos.z);
        }
    }
}

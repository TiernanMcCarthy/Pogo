using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    protected virtual void KillPlayer()
    {
        SpawnManager.instance.SpawnPlayer();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody.gameObject==PlayerManagement.player.gameObject)
        {
            KillPlayer();
            return;
        }

        if (collision.rigidbody != null)
        {
            Interactable interact = collision.rigidbody.gameObject.GetComponent<Interactable>();

            if (interact != null)
            {
                interact.OnDisposal();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.attachedRigidbody != null)
        {
            if (collision.attachedRigidbody.gameObject == PlayerManagement.player.gameObject)
            {
                KillPlayer();
            }

            Interactable interact = collision.attachedRigidbody.gameObject.GetComponent<Interactable>();

            if (interact != null)
            {
                interact.OnDisposal();
            }
        }
    }
}

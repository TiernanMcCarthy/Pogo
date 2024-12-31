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
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.attachedRigidbody.gameObject == PlayerManagement.player.gameObject)
        {
            KillPlayer();
        }
    }
}

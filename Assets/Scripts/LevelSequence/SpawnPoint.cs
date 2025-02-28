using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool canBeTriggered = true;

    private void OnTriggerEnter(Collider other)
    {
        if(!canBeTriggered || other.attachedRigidbody==null)
        {
            return;
        }
        if(other.attachedRigidbody.gameObject==PlayerManagement.player.gameObject)
        {
            SpawnManager.instance.AssignSpawnPoint(this);
            canBeTriggered = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}

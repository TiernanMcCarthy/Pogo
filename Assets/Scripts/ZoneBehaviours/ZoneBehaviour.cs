using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Zone behaviour can and should implement behaviours to the player or the scene objects specified when the player is inside the zone
/// </summary>
public class ZoneBehaviour : MonoBehaviour
{
    public List<Rigidbody> rigidBodiesInZone;

    protected virtual void OnRigidBodyAdded(Rigidbody rigid)
    {

    }

    protected virtual void OnRigidBodyRemoved(Rigidbody rigid)
    {

    }

    public void ManageZone()
    {
        Execute();
    }
    protected virtual void Execute()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody!=null)
        {
            rigidBodiesInZone.Add(other.attachedRigidbody);
            OnRigidBodyAdded(other.attachedRigidbody);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(rigidBodiesInZone.Contains(other.attachedRigidbody))
        {
            rigidBodiesInZone.Remove(other.attachedRigidbody);
        }

        //Sometimes a zone might remove this rigidbody from its list, e.g. the player, but informing it left is still useful
        OnRigidBodyRemoved(other.attachedRigidbody);
    }

}

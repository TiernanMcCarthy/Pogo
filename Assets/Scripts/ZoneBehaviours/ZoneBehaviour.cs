using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZoneBehaviour
{

    [System.Serializable]
    public class ColliderRigidbodyReference
    {
        public List<Collider> collidersInZone;
        public Rigidbody rigid;

        public ColliderRigidbodyReference(Rigidbody rig)
        {
            rigid = rig;
            collidersInZone = new List<Collider>();
        }
    }

    /// <summary>
    /// A Zone behaviour can and should implement behaviours to the player or the scene objects specified when the player is inside the zone
    /// </summary>
    public class ZoneBehaviour : MonoBehaviour
    {
        public List<ColliderRigidbodyReference> rigidColliders;

        [SerializeField] private bool activateOnce = false;

        protected bool activatedBefore = false;

        protected virtual void OnRigidBodyAdded(ColliderRigidbodyReference rigidContainer)
        {

        }

        protected virtual void OnRigidBodyRemoved(ColliderRigidbodyReference rigid)
        {

        }
        public void Start()
        {
            rigidColliders = new List<ColliderRigidbodyReference>();
        }
        public void ManageZone()
        {
            Execute();
        }
        protected virtual void Execute()
        {

        }

        /// <summary>
        /// This is called the first time a rigidbody enters a zone
        /// </summary>
        /// <param name="rigid"></param>
        protected virtual void FirstEntry(ColliderRigidbodyReference rigid)
        {

        }

        //This checks rigidbodies and their coliders for presence and updates based on the situation
        protected void CheckCollider(Collider col, Rigidbody target, out ColliderRigidbodyReference colliderRigidbody, bool removed)
        {
            colliderRigidbody = null;
            for (int i = 0; i < rigidColliders.Count; i++)
            {
                //We've found our target, it already exists, check and update collider references
                if (target == rigidColliders[i].rigid)
                {
                    colliderRigidbody = rigidColliders[i];
                    if (!removed)
                    {
                        if (!colliderRigidbody.collidersInZone.Contains(col))
                        {
                            colliderRigidbody.collidersInZone.Add(col);
                        }
                    }
                    else
                    {
                        if (colliderRigidbody.collidersInZone.Contains(col))
                        {
                            colliderRigidbody.collidersInZone.Remove(col);
                            if (colliderRigidbody.collidersInZone.Count == 0)
                            {
                                rigidColliders.Remove(colliderRigidbody);
                            }
                        }
                    }
                    return;
                }
            }

            if (!removed)
            {
                colliderRigidbody = new ColliderRigidbodyReference(target);
                colliderRigidbody.collidersInZone.Add(col);
                FirstEntry(colliderRigidbody);
                rigidColliders.Add(colliderRigidbody);
            }
            return;
        }

        protected void CheckCollider(Collider col, Rigidbody target, bool removed)
        {
            ColliderRigidbodyReference colliderRigidbody = null;
            for (int i = 0; i < rigidColliders.Count; i++)
            {
                //We've found our target, it already exists, check and update collider references
                if (target == rigidColliders[i].rigid)
                {
                    colliderRigidbody = rigidColliders[i];
                    if (!removed)
                    {
                        if (!colliderRigidbody.collidersInZone.Contains(col))
                        {
                            colliderRigidbody.collidersInZone.Add(col);
                        }
                    }
                    else
                    {
                        if (colliderRigidbody.collidersInZone.Contains(col))
                        {
                            colliderRigidbody.collidersInZone.Remove(col);
                            if (colliderRigidbody.collidersInZone.Count == 0)
                            {
                                rigidColliders.Remove(colliderRigidbody);
                            }
                        }
                    }
                    return;
                }
            }

            if (!removed)
            {
                colliderRigidbody = new ColliderRigidbodyReference(target);
                colliderRigidbody.collidersInZone.Add(col);
                rigidColliders.Add(colliderRigidbody);
                FirstEntry(colliderRigidbody);
            }
            return;
        }

        public void OnTriggerEnter(Collider other)
        {
            if(activatedBefore&& activateOnce|| other.isTrigger)
            {
                return;
            }

            
            if (other.attachedRigidbody != null)
            {
                ColliderRigidbodyReference colReference = null;
                CheckCollider(other, other.attachedRigidbody, out colReference, false);
                OnRigidBodyAdded(colReference);
                activatedBefore = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody != null)
            {
                ColliderRigidbodyReference colReference = null;
                CheckCollider(other, other.attachedRigidbody, out colReference, true);

                if(colReference==null)
                {
                    return;
                }
                if (colReference.collidersInZone.Count == 0)
                {
                    OnRigidBodyRemoved(colReference);
                }

            }
        }

    }
}

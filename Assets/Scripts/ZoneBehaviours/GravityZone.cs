using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : ZoneBehaviour
{
    [SerializeField] private Vector3 gravityOverride;

#if UNITY_EDITOR
    [Header("EditorOnly")]
    [SerializeField] private float gravityForce;
    [ProButton]
    public void SetGravityDirWithForce()
    {
        gravityOverride = transform.up * -1 * gravityForce;
    }

    [ProButton]
    public void SetGravityDirDefault()
    {
        gravityOverride = transform.up *Physics.gravity.y;
    }


#endif


    protected override void FirstEntry(ColliderRigidbodyReference rigid)
    {
        rigid.rigid.velocity = rigid.rigid.velocity * 0.5f;
        Vector3 dir = rigid.rigid.transform.position - transform.position;
        rigid.rigid.AddForce(gravityOverride.magnitude *-dir.normalized*10);
    }
    protected override void OnRigidBodyAdded(ColliderRigidbodyReference rigidContainer)
    {
        if(PlayerManagement.player.gameObject== rigidContainer.rigid.gameObject)
        {
            //rigidBodiesInZone.Remove(rigid);
            PlayerManagement.player.SetGravity(gravityOverride);
        }
        rigidContainer.rigid.useGravity = false;
    }

    protected override void OnRigidBodyRemoved(ColliderRigidbodyReference rigidContainer)
    {
        if(rigidContainer.rigid.gameObject!=PlayerManagement.player.gameObject)
        {
            rigidContainer.rigid.useGravity = true;
        }
        else
        {
            PlayerManagement.player.SetGravity(GravityManager.sceneGravity);
        }
        /*
        if (rigid.gameObject != PlayerManagement.player.gameObject)
        {
            rigid.useGravity = true;
        }
        else
        {
            // work something out better, maybe we'll have mixed triggerZones?
            PlayerManagement.player.SetGravity(GravityManager.sceneGravity);
        }*/
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        foreach(ColliderRigidbodyReference rigid in rigidColliders)
        {
            if (PlayerManagement.player.gameObject != rigid.rigid.gameObject)
            {
                rigid.rigid.AddForce(gravityOverride);
            }
        }
    }

    protected override void Execute()
    {
        
    }
}

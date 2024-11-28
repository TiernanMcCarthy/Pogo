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



    protected override void OnRigidBodyAdded(Rigidbody rigid)
    {
        if(PlayerManagement.player.gameObject==rigid.gameObject)
        {
            rigidBodiesInZone.Remove(rigid);
            PlayerManagement.player.SetGravity(gravityOverride);
        }
        rigid.useGravity = false;
    }

    protected override void OnRigidBodyRemoved(Rigidbody rigid)
    {
        if (rigid.gameObject != PlayerManagement.player.gameObject)
        {
            rigid.useGravity = true;
        }
        else
        {
            // work something out better, maybe we'll have mixed triggerZones?
            PlayerManagement.player.SetGravity(GravityManager.sceneGravity);
        }
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        foreach(Rigidbody rigid in rigidBodiesInZone)
        {
            rigid.AddForce(gravityOverride);
        }
    }

    protected override void Execute()
    {
        
    }
}

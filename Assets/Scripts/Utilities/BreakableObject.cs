using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundObject))]
public class BreakableObject : MonoBehaviour
{
    [SerializeField] private float forceToBreak = 10.0f;

    [SerializeField] private float slowDownFactor = 0.9f;

    [SerializeField] private float breakoffItemSpeedFactor = 0.2f;

    [SerializeField] private List<Rigidbody> breakoffRigidbodies;

    [SerializeField] private SoundObject soundManager;

    private MeshRenderer meshRenderer;

    private Collider attachedCollider;

    // Start is called before the first frame update
    void Start()
    {
        soundManager=GetComponent<SoundObject>();
        attachedCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnBreakOff(Vector3 relativeVelocity)
    {
        if(breakoffRigidbodies==null)
        {
            return;
        }

        foreach(var b in breakoffRigidbodies)
        {
            b.gameObject.SetActive(true);
            b.velocity=relativeVelocity * breakoffItemSpeedFactor;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.relativeVelocity.magnitude > forceToBreak)
            {
                meshRenderer.enabled = false;
                attachedCollider.enabled = false;
                collision.rigidbody.velocity =collision.relativeVelocity * slowDownFactor;
                SpawnBreakOff(collision.relativeVelocity);
                soundManager.PlayDestructionSound();
            }
        }
    }
}

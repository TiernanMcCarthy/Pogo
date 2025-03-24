using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyVelocity : MonoBehaviour
{
    [SerializeField] private float velocityPercentageCopy = 0.98f;

    [SerializeField] private float lerpSpeed = 0.5f;

    float lerp;

    private Rigidbody rb;

    private Rigidbody playerCopy;

    public static void CancelAllVelocityCopies()
    {
        CopyVelocity[] temp= FindObjectsOfType<CopyVelocity>();

        foreach(CopyVelocity t in temp)
        {
            t.enabled = false;
            t.rb.mass = 0.1f;
            t.transform.parent= null;
            t.transform.localScale = Vector3.one * 100;
            t.tag = "Magnetic";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void ActivateRigidBody()
    {
        rb.isKinematic = false;
        rb.velocity = PlayerManagement.player.GetVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            rb.velocity = PlayerManagement.player.GetVelocity().magnitude * Vector3.down * lerp;
            lerp = Mathf.Clamp(lerp + lerpSpeed, 0.01f, velocityPercentageCopy);
        }
    }
}

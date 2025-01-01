using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public enum MovementBehaviours
{
    LOOP,
    STOP,
    REVERSE
}

[RequireComponent(typeof(SplineContainer))]
public class FollowSpline : ZoneBehaviour
{
    [SerializeField] private SplineContainer splines;

    [SerializeField] private MovementBehaviours movementBehaviours;

    [SerializeField]private Rigidbody rigid;

    [SerializeField] private float speed;

    [SerializeField] private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        previousPos = transform.position;
        //splines = GetComponent<SplineContainer>();
       // rigid= GetComponent<Rigidbody>();
        knotTarget = splines.transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
        
    }
    protected override void OnRigidBodyAdded(ColliderRigidbodyReference rigidContainer)
    {
        
    }

    protected override void OnRigidBodyRemoved(ColliderRigidbodyReference rigidContainer)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    int splineIndex = 0;
    int knotIndex = 0;

    Vector3 knotTarget = Vector3.zero;

    void ResetSpline()
    {
        splineIndex = 0;
        knotIndex = 0;

        knotTarget = splines.transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
    }
    void EvaluateNextTarget()
    {
        knotIndex++;
        if (splineIndex > splines.Splines.Count()-1)
        {
            ResetSpline();
            return;
        }

        if (knotIndex >= splines[splineIndex].Knots.Count()-1)
        {
            knotIndex = 0;
            splineIndex++;
        }
        knotTarget = splines.transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
    }
    Vector3 previousPos;
    private void FixedUpdate()
    {
        if (isActive)
        {
            rigid.velocity = (knotTarget - rigid.transform.position).normalized * speed;
            //rigid.transform.position+=(knotTarget- rigid.transform.position).normalized * speed;
            //rigid.MovePosition(previousPos + (knotTarget - rigid.transform.position).normalized * speed);
            if (Vector3.Distance(rigid.transform.position, knotTarget) < 0.2f)
            {
                EvaluateNextTarget();
            }

            foreach(ColliderRigidbodyReference colliderRigidbody in rigidColliders)
            {
               // colliderRigidbody.rigid.AddForce((knotTarget - rigid.transform.position).normalized * speed);
            }
            previousPos = transform.position;
        }
    }
}

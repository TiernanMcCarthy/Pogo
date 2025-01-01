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
public class FollowSpline : MonoBehaviour
{
    private SplineContainer splines;

    [SerializeField] private MovementBehaviours movementBehaviours;

    [SerializeField]private Rigidbody rigid;

    [SerializeField] private float speed;

    [SerializeField] private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        splines = GetComponent<SplineContainer>();
       // rigid= GetComponent<Rigidbody>();
        knotTarget = transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
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

        knotTarget = transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
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
        knotTarget = transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
    }
    private void FixedUpdate()
    {
        if (isActive)
        {
            rigid.transform.position+=(knotTarget- rigid.transform.position).normalized * speed;
            if (Vector3.Distance(rigid.transform.position, knotTarget) < 0.2f)
            {
                EvaluateNextTarget();
            }
        }
    }
}

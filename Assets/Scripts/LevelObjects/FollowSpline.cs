using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public enum MovementBehaviours
{
    LOOP,
    STOP,
    REVERSE,
    INSTANTRESET
}

[RequireComponent(typeof(Rigidbody))]
public class FollowSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer splines;

    [SerializeField] private MovementBehaviours movementBehaviour;

    [SerializeField]private Rigidbody rigid;

    [SerializeField] private float speed;

    [SerializeField] private bool isActive = true;

    [SerializeField] private float waitBetweenResets = 0;
    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
        knotTarget = splines.transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int splineIndex = 0;
    int knotIndex = 0;

    Vector3 knotTarget = Vector3.zero;

    IEnumerator ResetSpline(float waitTime)
    {
        isActive = false;
        rigid.isKinematic = true;
        yield return new WaitForSeconds(waitTime);
        isActive= true;
        rigid.isKinematic= false;
        if(movementBehaviour==MovementBehaviours.LOOP)
        {
            splineIndex = 0;
            knotIndex = 0;
        }
        else if (movementBehaviour == MovementBehaviours.INSTANTRESET)
        {
            splineIndex = 0;
            knotIndex = 0;
            transform.position = splines.transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
        }

        knotTarget = splines.transform.TransformPoint(splines[splineIndex].Knots.ElementAt(knotIndex).Position);
    }
    void EvaluateNextTarget()
    {
        knotIndex++;

        if (knotIndex > splines[splineIndex].Knots.Count()-1)
        {
            knotIndex = 0;
            splineIndex++;
        }
        if (splineIndex > splines.Splines.Count() - 1)
        {
            StartCoroutine(ResetSpline(waitBetweenResets));
            return;
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

            previousPos = transform.position;
        }
    }
}

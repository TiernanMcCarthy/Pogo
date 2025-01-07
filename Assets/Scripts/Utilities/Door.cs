using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour , Actionable
{

    private Vector3 startPosition;

    private Vector3 targetPos;

    [SerializeField] private Transform endPoint;

    [SerializeField] private Transform doorMesh;

    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private AudioSource doorAudioEmitter;

    public void FailAction()
    {
        targetPos = startPosition;
    }

    public void ResetAction()
    {
        throw new System.NotImplementedException();
    }

    public void SuccessAction()
    {
        targetPos = endPoint.position;
    }


    private void Start()
    {
        startPosition = doorMesh.position;
        targetPos = startPosition;
    }

    public void Update()
    {
        if(Vector3.Distance(doorMesh.position, targetPos)>0.1f)
        {
            if(!doorAudioEmitter.isPlaying)
            {
                doorAudioEmitter.Play();
            }
            Vector3 direction = (targetPos - doorMesh.position).normalized;
            doorMesh.transform.position += direction * Time.deltaTime * moveSpeed;
        }
        else
        {
            doorAudioEmitter.Stop();
            doorMesh.transform.position = targetPos;
        }
    }
}

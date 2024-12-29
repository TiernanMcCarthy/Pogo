using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{

    [SerializeField] private Transform followTransform;

    private Vector3 lastRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 newRotation = followTransform.transform.rotation.eulerAngles;

        Vector3 difference = newRotation - lastRotation;

        Vector3 eulerRot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(eulerRot + difference);

        lastRotation = transform.rotation.eulerAngles;
    }
}

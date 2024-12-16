using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    [SerializeField] private Vector3 rotationAxis;

    private Vector3 rotations;
    // Start is called before the first frame update
    void Start()
    {
        rotations = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rotations += rotationAxis.normalized * rotateSpeed;
        transform.localRotation = Quaternion.Euler(rotations);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOffset : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float yOffset;

    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position
        Vector3 targetPos=new Vector3(target.position.x, target.position.y + yOffset, target.transform.position.z);

        float distance = Vector3.Distance(transform.position,targetPos);

        if(distance < speed*Time.deltaTime)
        {
            transform.position= targetPos;
        }
        else
        {
            transform.position += speed * Time.deltaTime * (targetPos- transform.position).normalized;
        }

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField] private bool canBeDestroyed = true;

    [SerializeField] private bool respawn;

    private Rigidbody rb;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDisposal()
    {
        if(canBeDestroyed)
        {
            Destroy(gameObject);
        }
        else if(respawn)
        {
            rb.velocity = Vector3.zero;
            transform.position = startPos;
        }
    }
}

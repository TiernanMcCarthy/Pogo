using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneButton : LevelComponent
{
    [SerializeField]
    public UnityEvent OnPressed;

    [SerializeField]
    public UnityEvent OnReleased;

    [SerializeField]
    private bool stayPressed;

    [SerializeField]
    private Transform groundCheck;

    private Rigidbody rb;

    [SerializeField]
    public bool isPressed {  get; private set; }

    [SerializeField]
    private LayerMask ignoreMask;

    [Header("FloatingButton Forces")]
    //desired floating height off of the ground, all forces work together to achieve this
    [SerializeField] private float floatHeight;
    [SerializeField] private float springStrength;
    [SerializeField] private float springDamper;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FloatButton()
    {
        RaycastHit hit;

        LayerMask avoid = ~ignoreMask;
        // Cast ray down
        if (Physics.Raycast(groundCheck.position, transform.up * -1, out hit, floatHeight))
        {
            // Get the distance between the button and the surface
            float currentHeight = hit.distance;

            // Calculate the height difference from the desired floatHeight
            float heightDifference = currentHeight - floatHeight;

            Vector3 velocity = rb.velocity;

            //Calculate and apply spring force relative to button orientation
            float springForce = heightDifference * -springStrength - velocity.magnitude * springDamper;

            rb.AddForce(transform.up * springForce, ForceMode.Acceleration);

            if(currentHeight<floatHeight*0.1f)
            {
                isPressed = true;
            }
            else
            {
                isPressed = false;
            }
        }
    }

    private void FixedUpdate()
    {
        FloatButton();
    }
}

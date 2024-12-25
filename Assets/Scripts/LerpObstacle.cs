using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpObstacle : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField]private float lerpPos = 0;

    public bool isMoving {  get; private set; }

    [SerializeField]private Transform targetA;
    [SerializeField] private Transform targetB;
    // Start is called before the first frame update
    void Start()
    {
        if (targetA != null && targetB != null)
        { 
            //transform.position = targetA.position;
            
        }
    }

    #if UNITY_EDITOR
    [ProButton]
    public void SetPosition()
    {
        if (targetA != null && targetB != null)
        {
            transform.position = Vector3.Lerp(targetA.position, targetB.position, lerpPos);
        }
        else
        {
            Debug.LogError("Target A or/and B is not assigned on " + gameObject.name);
        }
    }
#endif

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            
        }
    }

    public void ActivateObstacle()
    {
        isMoving = true;
    }

    public void DeactivateObstacle()
    {
        isMoving = false;
    }
}

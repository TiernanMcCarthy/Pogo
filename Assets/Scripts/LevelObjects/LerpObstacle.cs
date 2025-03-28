using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpObstacle : Obstacle
{
    [SerializeField] private float speed;

    [SerializeField] private float lerpPos = 0;

    [SerializeField] private int direction = 1;

    [SerializeField] private bool m_isMoving = true;
    public bool isMoving { get { return m_isMoving; } private set { m_isMoving = value; } }

    [SerializeField]private Transform targetA;
    [SerializeField] private Transform targetB;


    [SerializeField] private bool waitToStart = false;
    [SerializeField] private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        if(waitToStart)
        {
            StartCoroutine(WaitToStart());
        }
    }

    private IEnumerator WaitToStart()
    {
        m_isMoving = false;
        yield return new WaitForSeconds(waitTime);
        m_isMoving = true;
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
        if(m_isMoving)
        {
            transform.position= Vector3.Lerp(targetA.position,targetB.position,lerpPos);
            lerpPos += speed *direction* Time.deltaTime;
            if(lerpPos>1 || lerpPos <0)
            {
                direction *= -1;
                lerpPos=Mathf.RoundToInt(lerpPos);
            }
        }
    }

    public void ActivateObstacle()
    {
        m_isMoving = true;
    }

    public void DeactivateObstacle()
    {
        m_isMoving = false;
    }
}

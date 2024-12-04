using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPressed : MonoBehaviour, ITracker
{
    [SerializeField]
    //private List<>
    public event Action OnTrackerCompleted;

    public bool IsComplete()
    {
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System;
using UnityEngine;
using UnityEngine.Events;
public abstract class Tracker :MonoBehaviour
{
    public bool isComplete {  get; private set; }

    [SerializeField] private bool executeEveryFrame;

    [Space(10,order =100)]
    [SerializeField] private UnityEvent completionEvents;

    public void ForceCompletion()
    {
        if (!isComplete)
        {
            isComplete = true;
            completionEvents?.Invoke();
        }
    }

    public bool ExecutesEveryFrame()
    {
        return executeEveryFrame;
    }

    //Trackers might and can affect the world, they should process if their sequence is active
    public abstract void ProcessTracker();

    //Trackers should check if they're completed after being processed
    public abstract bool CheckCompletion();
}

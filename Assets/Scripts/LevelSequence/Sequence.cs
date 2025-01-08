using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Sequences manage and control "Rooms" where tasks take place. Once all its trackers have been completed, it should complete its end actions
/// </summary>
public class Sequence : MonoBehaviour
{
    [SerializeField]
    private List<Sequence> nextSequence;

    [SerializeField]
    private List<Tracker> trackersToComplete;

    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    private UnityEvent startActions;

    [SerializeField]
    private UnityEvent endActions;

    [SerializeField]
    private UnityEvent completionActions;

    [SerializeField] private UnityEvent failActions;

    private bool hasCompleted = false;
    
    public void ActivateSequence()
    {
        isActive=true;
    }

    private void CheckTrackers()
    {
        int completeTrackers = 0;
        for (int i = 0; i < trackersToComplete.Count; ++i)
        {
            if (trackersToComplete[i].ExecutesEveryFrame())
            {
                if (trackersToComplete[i].CheckCompletion())
                {
                    completeTrackers++;
                }
            }

            if(completeTrackers==trackersToComplete.Count && !hasCompleted) //Completetion Events are called once upon all trackers being met
            {
                completionActions.Invoke();
                hasCompleted = true;
            }
            else if(completeTrackers<trackersToComplete.Count && hasCompleted) //Fail events should only happen when the 
            {
                failActions.Invoke();
                hasCompleted = false;
            }
        }
    }

    private void ProcessTrackers()
    {
        for (int i = 0; i < trackersToComplete.Count; ++i)
        {
            trackersToComplete[i].ProcessTracker();
        }
    }


    public void Update()
    {
        if(isActive)
        {
            ProcessTrackers();
            CheckTrackers();
        }
    }
}

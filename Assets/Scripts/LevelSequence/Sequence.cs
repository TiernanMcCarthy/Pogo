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

    
    public void ActivateSequence()
    {
        isActive=true;
    }

    private void CheckTrackers()
    {
        int completeTrackers = 0;
        for (int i = 0; i < trackersToComplete.Count; ++i)
        {
            if (trackersToComplete[i].ExecuteEveryFrame)
            {
                trackersToComplete[i].CheckCompletion();
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

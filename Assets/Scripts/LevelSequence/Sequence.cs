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

    [SerializeField] private float waitToComplete = 0;


    private bool completing;
    public void ActivateSequence()
    {
        isActive=true;
    }

    private int GetCompleteTrackers()
    {
        int count = 0;
        for (int i = 0; i < trackersToComplete.Count; ++i)
        {
            if (trackersToComplete[i].ExecutesEveryFrame())
            {
                if (trackersToComplete[i].CheckCompletion())
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void CheckTrackers()
    {
        int completeTrackers = GetCompleteTrackers();


        if (completing == false && trackersToComplete.Count == completeTrackers && !hasCompleted)
        {
            completing = true;
            StartCoroutine(CompletionCheck());
            hasCompleted = true;
            return;
        }

        if (completeTrackers < trackersToComplete.Count && hasCompleted) //Fail events should only happen when the 
        {
            failActions.Invoke();
            StopCoroutine(CompletionCheck());
            hasCompleted = false;
        }
    }

    private IEnumerator CompletionCheck()
    {
        yield return new WaitForSeconds(waitToComplete);

        int completeTrackers = GetCompleteTrackers();

        if(completeTrackers==trackersToComplete.Count)
        {
            completionActions.Invoke();
        }

        completing = false;
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

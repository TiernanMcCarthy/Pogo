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
    private List<Sequence> nextSequences;

    [SerializeField]
    private List<ITracker> trackersToComplete;

    [SerializeField]
    private UnityEvent endActions;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

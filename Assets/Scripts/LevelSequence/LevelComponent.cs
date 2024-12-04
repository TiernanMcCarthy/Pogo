using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ResetComponent()
    {
        Debug.LogError("No Reset Implemented");
    }
}

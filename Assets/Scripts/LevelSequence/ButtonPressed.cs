using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonsPressed : Tracker
{
    [SerializeField]
    private List<SceneButton> buttonList;

    public bool IsComplete()
    {
        foreach (SceneButton button in buttonList)
        {
            if(!button.isPressed)
            {
                return false;
            }
        }

        return true;
    }

    public override void ProcessTracker()
    {
        throw new NotImplementedException();
    }

    public override bool CheckCompletion()
    {
        foreach (SceneButton button in buttonList)
        {
            if (!button.isPressed)
            {
                return false;
            }
        }

        return true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Actionable
{
    public abstract void SuccessAction();

    public abstract void FailAction();

    public abstract void ResetAction();

}

using System;
public interface ITracker
{
    bool IsComplete();
    event Action OnTrackerCompleted;  // Notify when this tracker is done
}

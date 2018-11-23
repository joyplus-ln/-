using System.Collections.Generic;

public class FastTimerManager : Manager
{
    private List<FastTimer> _timers = new List<FastTimer>();

    // buffer adding timers so we don't edit a collection during iteration
    private List<FastTimer> _timersToAdd = new List<FastTimer>();

    public void RegisterTimer(FastTimer timer)
    {
        this._timersToAdd.Add(timer);
    }

    public void CancelAllTimers()
    {
        foreach (FastTimer timer in this._timers)
        {
            timer.Cancel();
        }

        this._timers = new List<FastTimer>();
        this._timersToAdd = new List<FastTimer>();
    }

    public void PauseAllTimers()
    {
        foreach (FastTimer timer in this._timers)
        {
            timer.Pause();
        }
    }

    public void ResumeAllTimers()
    {
        foreach (FastTimer timer in this._timers)
        {
            timer.Resume();
        }
    }

    // update all the registered timers on every frame

    public override void update()
    {
        base.update();
        UpdateAllTimers();
    }

    private void UpdateAllTimers()
    {
        if (this._timersToAdd.Count > 0)
        {
            this._timers.AddRange(this._timersToAdd);
            this._timersToAdd.Clear();
        }

        if (this._timers.Count > 0)
            foreach (FastTimer timer in this._timers)
            {
                timer.Update();
            }

        this._timers.RemoveAll(t => t.isDone);
    }
}
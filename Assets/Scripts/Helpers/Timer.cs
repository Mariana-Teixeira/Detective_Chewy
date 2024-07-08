using System;
using UnityEngine;

public class Timer
{
    public Action TimerOver;

    public bool _runTimer = false;
    private float _maximumTime;
    public float CurrentTime;

    public Timer()
    {
        TimerOver += StopTimer;
    }

    public void StartTimer(float maximumTime)
    {
        _maximumTime = maximumTime;
        CurrentTime = _maximumTime;
        _runTimer = true;
    }

    public void StopTimer()
    {
        _runTimer = false;
        _maximumTime = 0f;
    }

    public void TickTime()
    {
        if (_runTimer) CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f) TimerOver.Invoke();
    }
}

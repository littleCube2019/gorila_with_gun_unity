using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils>
{
    public IEnumerator SetTimer(Action action, float seconds)
    {
        IEnumerator task = TimerCoroutine(action, seconds);
        StartCoroutine(task);
        return task;
    }
    IEnumerator TimerCoroutine(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    public IEnumerator SetPeriodTask(Action action, float period)
    {
        IEnumerator task = CircularCoroutine(action, period);
        return task;
    }

    IEnumerator CircularCoroutine(Action action, float period)
    {
        while (true)
        {
            yield return new WaitForSeconds(period);
            action();
        }
    }
}

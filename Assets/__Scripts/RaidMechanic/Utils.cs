using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static string FormatTime(float totalSeconds)
    {
        return $"{(int)totalSeconds / 60:00}:{(int)totalSeconds % 60:00}";
    }

    public static IEnumerator ExecuteFunctionAfterDelay(float delay, IEnumerator coroutineToExecute, MonoBehaviour host)
    {
        yield return new WaitForSeconds(delay);
        host.StartCoroutine(coroutineToExecute);
    }

    public static IEnumerator ExecuteCoroutineAfterDelay(float delay, Action functionToExecute)
    {
        yield return new WaitForSeconds(delay);
        functionToExecute?.Invoke();
    }
}

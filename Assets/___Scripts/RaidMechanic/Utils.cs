using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    public static string FormatTime(float totalSeconds)
    {
        return $"{(int)totalSeconds / 60:00}:{(int)totalSeconds % 60:00}";
    }

    public static IEnumerator ExecuteCoroutineAfterDelay(float delay, IEnumerator coroutineToExecute, MonoBehaviour host)
    {
        yield return new WaitForSeconds(delay);
        host.StartCoroutine(coroutineToExecute);
    }

    public static IEnumerator ExecuteFunctionAfterDelay(float delay, Action functionToExecute)
    {
        yield return new WaitForSeconds(delay);
        functionToExecute?.Invoke();
    }

    public static void ShuffleList<T>(List<T> listToShuffle)
    {
        int n = listToShuffle.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            
            // Swap elements
            T value = listToShuffle[k];
            listToShuffle[k] = listToShuffle[n];
            listToShuffle[n] = value;
        }
    }
}

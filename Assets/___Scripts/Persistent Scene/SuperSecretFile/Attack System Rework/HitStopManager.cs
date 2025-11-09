using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;
    private bool waiting;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Stop(float duration)
    {
        if (waiting)
            return;

        StartCoroutine(DoHitStop(duration));
    }
    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSeconds(duration);  
        Time.timeScale = 1.0f;
        waiting = false;
    }
    private IEnumerator DoHitStop(float duration)
    {
        waiting = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        waiting = false;
    }
}

using TMPro;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RaidController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI raidAlertText;
    [SerializeField]
    private TextMeshProUGUI timerText;

    private TextController _raidAlertTextController;
    private TextController _timerTextController;

    private const float _waveDurationInSeconds = 30;
    private float _timeBeforeNextWaveInSeconds = 0;

    private float _tickTimer = 0f;
    

    void Awake()
    {
        _raidAlertTextController = new TextController(raidAlertText);
        _timerTextController = new TextController(timerText);
    }

    void Start()
    {
        
    }

    void Update()
    {
        updateTimer();
    }

    private void updateTimer()
    {
        _tickTimer += Time.deltaTime;
        if (_tickTimer >= 1f)
        {
            _tickTimer = 0f;
            _timeBeforeNextWaveInSeconds--;
            if (_timeBeforeNextWaveInSeconds >= 0)
            {
                _timerTextController.SetText(FormatTime(_timeBeforeNextWaveInSeconds));
            }
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            _timeBeforeNextWaveInSeconds = _waveDurationInSeconds;
            _timerTextController.SetText(FormatTime(_timeBeforeNextWaveInSeconds));
            _timerTextController.SetTextVisible();
            StartCoroutine(ExecuteAfterDelay(_timeBeforeNextWaveInSeconds + 1, _timerTextController.SetTextInvisible));
            _raidAlertTextController.SetTextVisible();
            StartCoroutine(ExecuteAfterDelay(2f, _raidAlertTextController.FadeOut(1f)));
        }
    }
    
    private string FormatTime(float totalSeconds) =>
        $"{(int)totalSeconds / 60:00}:{(int)totalSeconds % 60:00}";


    public IEnumerator ExecuteAfterDelay(float delay, IEnumerator coroutineToExecute)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(coroutineToExecute); 
    }

    public IEnumerator ExecuteAfterDelay(float delay, Action functionToExecute)
    {
        yield return new WaitForSeconds(delay);
        functionToExecute?.Invoke();
    }



    
}

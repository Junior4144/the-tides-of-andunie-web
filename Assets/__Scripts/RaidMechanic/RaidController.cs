using TMPro;
using UnityEngine;

public class RaidController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI preRaidText;
    [SerializeField]
    private TextMeshProUGUI timerText;

    private TextController _preRaidTextController;
    private TextController _timerTextController;

    private const float _waveDurationInSeconds = 30;
    private float _timeBeforeNextWaveInSeconds = 0;

    private float _tickTimer = 0f;
    

    void Awake()
    {
        _preRaidTextController = new TextController(preRaidText);
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
                _timerTextController.SetText(Utils.FormatTime(_timeBeforeNextWaveInSeconds));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            StartAndShowTimer();
            DisplayRaidAlertText();
            
        }
    }

    private void StartAndShowTimer()
    {
        _timeBeforeNextWaveInSeconds = _waveDurationInSeconds;
        _timerTextController.SetText(Utils.FormatTime(_timeBeforeNextWaveInSeconds));
        _timerTextController.SetTextVisible();
        StartCoroutine(Utils.ExecuteCoroutineAfterDelay(_timeBeforeNextWaveInSeconds + 1, _timerTextController.SetTextInvisible));
    }

    private void DisplayRaidAlertText()
    {
        _preRaidTextController.SetTextVisible();
        StartCoroutine(Utils.ExecuteFunctionAfterDelay(2f, _preRaidTextController.FadeOut(1f), this));
    }
}

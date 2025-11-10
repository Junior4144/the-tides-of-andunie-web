using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using System;

using Random = UnityEngine.Random;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class RaidController : MonoBehaviour
{

    // ------- TEXT -------
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI postRaidText;
    public Slider enemySlider;
    public GameObject SliderGameObject;

    private TextController _alertTextController;
    private TextController _timerTextController;
    private TextController _postRaidTextController;
    private TextController _enemiesRemainingTextController;

    // ------- TIME -------
    private float _masterTimer = 0f;

    // ------- WAVE CONFIG -------
    [SerializeField] private RaidConfig _raidConfig;
    public List<RewardListing> RaidCompletionRewards => _raidConfig.RaidCompletionRewards.RewardItems;
    [SerializeField] private List<Transform> _spawnPoints;

    private Queue<float> _wavesSpawnStartTimes = new();
    private List<WaveConfig> _wavesCurrentlySpawning = new();
    private List<GameObject> _spawnedEnemies = new();
    private int _totalNumberOfEnemies;

    // ------- STATES -------
    public enum RaidState { PreRaid, RaidInProgress, RaidComplete, RaidFailed }
    private RaidState _currentState;

    // ------- PUBLIC EVENTS -------
    public event Action OnRaidTriggered;
    public event Action OnRaidStart;
    public event Action OnRaidComplete;
    public event Action OnRaidFailed;
    public event Action OnRaidReset; // For the PreRaid state

    [Tooltip("If checked, the raid will NOT start automatically. It must be started by an external script (like a cutscene) calling BeginRaidSequence().")]
    [SerializeField] private bool waitForExternalSignal = false;


    private float _sliderSmoothSpeed = 5f;
    private float _sliderEnemySpawnCap = 0f;

    void Awake()
    {
        _alertTextController = new TextController(alertText);
        _timerTextController = new TextController(timerText);
        _postRaidTextController = new TextController(postRaidText);
        _totalNumberOfEnemies = _raidConfig.Waves.Sum(wave => wave.enemies.Sum(enemyData => enemyData.count));
        TransitionToPreRaidState();
        DisableEnemyBar();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _currentState == RaidState.PreRaid)
        {
            OnRaidTriggered?.Invoke();
            GetComponent<Collider2D>().enabled = false;
            if (!waitForExternalSignal)
            {
                BeginRaidSequence();
            }
        }
    }

    public void BeginRaidSequence()
    {
        TransitionToRaidInProgressState();
        var spawnStartTimesList = new List<float>();
        float schedulingTimePointer = _masterTimer;
        foreach (WaveConfig wave in _raidConfig.Waves)
        {
            float spawnStartTime = schedulingTimePointer + wave.countdown;
            StartCoroutine(ScheduleWave(wave, spawnStartTime));
            spawnStartTimesList.Add(spawnStartTime);
            schedulingTimePointer += wave.totalDuration;
        }

        spawnStartTimesList.Sort();
        _wavesSpawnStartTimes = new Queue<float>(spawnStartTimesList);
    }

    private IEnumerator ScheduleWave(WaveConfig wave, float spawnStartTime)
    {
        float alertTextTime = spawnStartTime - wave.countdown;
        yield return StartCoroutine(WaitUntilMasterTimer(alertTextTime));

        _alertTextController.SetText(wave.countDownText);
        DisplayTextThenFadeOut(_alertTextController);

        yield return StartCoroutine(WaitUntilMasterTimer(spawnStartTime));

        _alertTextController.SetText(wave.waveStartText);
        DisplayTextThenFadeOut(_alertTextController);

        yield return StartCoroutine(SpawnWaveEnemiesOverIntervals(wave));
    }
    private IEnumerator WaitUntilMasterTimer(float targetTime)
    {
        float waitTime = targetTime - _masterTimer;
        if (waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }

    void Update()
    {
        if (_currentState == RaidState.RaidInProgress)
        {
            TickMasterTimer();
            bool allEnemiesWereSpawned = AllEnemiesWereSpawned;
            bool allSpawnedEnemiesAreDead = AllSpawnedEnemiesAreDead;
            
            if (allEnemiesWereSpawned && allSpawnedEnemiesAreDead)
            {
                TransitionToRaidCompleteState();
            }
            else if (IsSpawning || (allEnemiesWereSpawned && !allSpawnedEnemiesAreDead))
            {
                _timerTextController.SetText("00:00");
                FlashTimerWhiteRed();
            }
            else // counting down
            {
                float timeTillNextSpawn = GetTimeTillNextWaveSpawn();

                if (timeTillNextSpawn <= 5f)
                {
                    FlashTimerWhiteRed();
                }
                else
                {
                    timerText.color = Color.white;
                }
                ShowTimer(timeTillNextSpawn);
            }
        }

        if (_currentState == RaidState.RaidInProgress)
        {
            int deadEnemies = _spawnedEnemies.Count(e => e == null);
            enemySlider.value = _totalNumberOfEnemies - deadEnemies;
        }
    }

    private void TickMasterTimer()
    {
        _masterTimer += Time.deltaTime;
    }

    private void FlashTimerWhiteRed()
    {
        bool isOddSeconds = ((int)_masterTimer & 1) == 1;
        timerText.color = isOddSeconds ? Color.white : Color.red;
    }

    private void ShowTimer(float time)
    {
        _timerTextController.SetText(Utils.FormatTime(time));
        _timerTextController.SetTextVisible();
    }
    
    private float GetTimeTillNextWaveSpawn()
    {
        while (_wavesSpawnStartTimes.Count > 0 && _wavesSpawnStartTimes.First() < _masterTimer)
        {
            _wavesSpawnStartTimes.Dequeue();
        }

        return _wavesSpawnStartTimes.Count == 0 ? 0f :  _wavesSpawnStartTimes.First() - _masterTimer;
    }

    private void TransitionToPreRaidState()
    {
        _currentState = RaidState.PreRaid;
        OnRaidReset?.Invoke();
    }

    private void TransitionToRaidInProgressState()
    {
        _masterTimer = 0f;
        _currentState = RaidState.RaidInProgress;

        ShowEnemiesBar();

        enemySlider.maxValue = _totalNumberOfEnemies;
        enemySlider.value = _totalNumberOfEnemies;

        OnRaidStart?.Invoke();
    }

    private void TransitionToRaidCompleteState()
    {
        _currentState = RaidState.RaidComplete;
        DisplayTextThenFadeOut(_postRaidTextController);
        RemoveEnemiesCountText();
        _timerTextController.SetTextInvisible();
        OnRaidComplete?.Invoke();
        DisableEnemyBar();
    }

    private void TransitionToRaidFailedState()
    {
        _currentState = RaidState.RaidFailed;
        OnRaidFailed?.Invoke();
    }

    private bool AllSpawnedEnemiesAreDead => !_spawnedEnemies.Any(enemy => enemy != null);
    private bool IsSpawning => _wavesCurrentlySpawning.Count > 0;

    private bool AllEnemiesWereSpawned => _totalNumberOfEnemies == _spawnedEnemies.Count;

    private void DisplayTextThenFadeOut(TextController controller)
    {
        controller.SetTextVisible();
        const float baseTime = 1.5f;
        const float wordsPerSecond = 4.0f;
        int wordCount = controller.Text.Split(' ').Length;
        float duration = baseTime + (wordCount / wordsPerSecond);
        StartCoroutine(Utils.ExecuteCoroutineAfterDelay(duration, controller.FadeOut(1f), this));
    }

    private void ShowEnemiesBar()
    {
        SliderGameObject.SetActive(true);
    }
    private void DisableEnemyBar()
    {
        SliderGameObject.SetActive(false);
    }


    private void RemoveEnemiesCountText() => StartCoroutine(_enemiesRemainingTextController.FadeOut(1f));

    private IEnumerator SpawnWaveEnemiesOverIntervals(WaveConfig wave)
    {
        if (_spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned for raid wave.");
            yield break;
        }

        _wavesCurrentlySpawning.Add(wave);

        List<GameObject> spawnPool = new();
        foreach (var enemyData in wave.enemies) 
        {
            for (int _ = 0; _ < enemyData.count; _++) spawnPool.Add(enemyData.prefab);
        }

        Utils.ShuffleList(spawnPool);

        foreach (GameObject enemyPrefab in spawnPool)
        {
            Transform chosenSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            GameObject newEnemy = Instantiate(enemyPrefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
            _spawnedEnemies.Add(newEnemy);
            _sliderEnemySpawnCap++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        _wavesCurrentlySpawning.Remove(wave);
    }

    public void EndRaid()
    {
        StopAllCoroutines(); // stop all waves
    }
}

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class RaidController : MonoBehaviour
{

    // ------- TEXT -------
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI postRaidText;
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;

    private TextController _alertTextController;
    private TextController _timerTextController;
    private TextController _postRaidTextController;
    private TextController _enemiesRemainingTextController;

    // ------- TIME -------
    private float _masterTimer = 0f;

    // ------- WAVE CONFIG -------
    [SerializeField] private RaidConfig raidConfig;
    [SerializeField] private List<Transform> spawnPoints;

    private Queue<float> _wavesSpawnStartTimes = new Queue<float>();
    private List<WaveConfig> _wavesCurrentlySpawning = new List<WaveConfig>();
    private List<GameObject> _spawnedEnemies = new List<GameObject>();
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

    void Awake()
    {
        _alertTextController = new TextController(alertText);
        _timerTextController = new TextController(timerText);
        _postRaidTextController = new TextController(postRaidText);
        _enemiesRemainingTextController = new TextController(enemiesRemainingText);
        _totalNumberOfEnemies = raidConfig.waves.Sum(wave => wave.enemies.Sum(enemyData => enemyData.count));
        TransitionToPreRaidState();
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
        foreach (WaveConfig wave in raidConfig.waves)
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
        float waitTime = spawnStartTime - _masterTimer;
        if (waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }
        

        _alertTextController.SetText(wave.waveStartText);
        DisplayTextThenFadeOut(_alertTextController);
        
        yield return StartCoroutine(SpawnWaveEnemiesOverIntervals(wave));
    }

    void Update()
    {
        if (_currentState == RaidState.RaidInProgress)
        {
            TickMasterTimer();
            ShowEnemiesCount();
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
        UpdateEnemiesCountText();
        ShowEnemiesCount();
        OnRaidStart?.Invoke();
    }

    private void TransitionToRaidCompleteState()
    {
        _currentState = RaidState.RaidComplete;
        DisplayTextThenFadeOut(_postRaidTextController);
        RemoveEnemiesCountText();
        _timerTextController.SetTextInvisible();
        OnRaidComplete?.Invoke();
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

    private void ShowEnemiesCount()
    {
        UpdateEnemiesCountText();
        _enemiesRemainingTextController.SetTextVisible();
    }

    private void UpdateEnemiesCountText()
    {
        // 1. Get current enemy count (Inefficient, but simple)
        int currentEnemies = _spawnedEnemies.Count(e => e != null);

        if (currentEnemies == 0)
        {
            _enemiesRemainingTextController.SetText(""); // Show nothing if no enemies
            return;
        }

        // --- THIS IS THE FIX ---
        // 2. Create a collection of 💀 strings and join them
        string skullText = string.Concat(Enumerable.Repeat("💀", currentEnemies));

        // 3. Set the text
        _enemiesRemainingTextController.SetText(skullText);
    }
    private void RemoveEnemiesCountText() => StartCoroutine(_enemiesRemainingTextController.FadeOut(1f));

    private IEnumerator SpawnWaveEnemiesOverIntervals(WaveConfig wave)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned for raid wave.");
            yield break;
        }

        _wavesCurrentlySpawning.Add(wave);

        List<GameObject> spawnPool = new List<GameObject>();
        foreach (var enemyData in wave.enemies) 
        {
            for (int _ = 0; _ < enemyData.count; _++) spawnPool.Add(enemyData.prefab);
        }

        Utils.ShuffleList(spawnPool);

        foreach (GameObject enemyPrefab in spawnPool)
        {
            Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject newEnemy = Instantiate(enemyPrefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
            _spawnedEnemies.Add(newEnemy);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        _wavesCurrentlySpawning.Remove(wave);
    }

    public void EndRaid()
    {
        StopAllCoroutines(); // stop all waves
    }
}

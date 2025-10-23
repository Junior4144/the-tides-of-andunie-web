using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using System;

using Random = UnityEngine.Random;

public class RaidController : MonoBehaviour
{

    // ------- TEXT -------
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI postRaidText;
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private TextMeshProUGUI prizePopupText;

    private TextController _alertTextController;
    private TextController _timerTextController;
    private TextController _postRaidTextController;
    private TextController _enemiesRemainingTextController;
    private TextController _prizePopupTextController;

    // ------- TIMERS -------
    private float _timeBeforeNextWaveInSeconds = 0;
    private float _tickTimer = 0f;

    // ------- CONFIG & STATE DATA -------
    [SerializeField] private RaidConfig raidConfig;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float _losingHealthPercentage = 0.15f;

    private int _currentWaveIndex;
    private List<GameObject> _activeEnemies = new List<GameObject>();
    public enum RaidState { PreRaid, PreWave, WaveInProgress, RaidComplete, RaidFailed}
    private RaidState _currentState;


    // ------- PUBLIC EVENTS -------
    public event Action OnRaidTriggered;
    public event Action OnPreWaveStart;
    public event Action OnWaveStart;
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
        _prizePopupTextController = new TextController(prizePopupText);
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
        if (_currentState == RaidState.PreRaid)
        {
            TransitionToPreWaveState();
        }
    }

    void Update()
    {
        if (_currentState == RaidState.PreWave)
        {
            UpdateTimer();

            if (_timeBeforeNextWaveInSeconds <= 0)
                TransitionToWaveInProgressState();
        }
        else if (_currentState == RaidState.WaveInProgress)
        {
            if (PlayerManager.Instance.GetPercentHealth() < _losingHealthPercentage)
            {
                TransitionToRaidFailedState();
                _activeEnemies.Clear();
                RemoveEnemiesCountText();
                return;
            }

            if (allCurrentWaveEnemiesSpawned && allActiveEnemiesAreDead)
            {
                if (_currentWaveIndex + 1 < raidConfig.waves.Count)
                    TransitionToPreWaveState();
                else
                    TransitionToRaidCompleteState();
                _activeEnemies.Clear();
                RemoveEnemiesCountText();
            }
            else
                UpdateEnemiesCountText();
        }
    }
    
    private void UpdateTimer()
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

    private void TransitionToPreRaidState()
    {
        _currentState = RaidState.PreRaid;
        _currentWaveIndex = -1;
        OnRaidReset?.Invoke();
    }

    private void TransitionToPreWaveState()
    {
        _currentState = RaidState.PreWave;
        StartNextWave();
        _alertTextController.SetText(currentWave.countDownText);
        DisplayTextThenFadeOut(_alertTextController);
        OnPreWaveStart?.Invoke();
    }

    private void TransitionToWaveInProgressState()
    {
        _currentState = RaidState.WaveInProgress;
        _alertTextController.SetText(currentWave.waveStartText);
        DisplayTextThenFadeOut(_alertTextController);
        StartCoroutine(SpawnWaveEnemiesOverIntervals());
        ShowEnemiesCount();
        OnWaveStart?.Invoke();
    }

    private void TransitionToRaidCompleteState()
    {
        _currentState = RaidState.RaidComplete;
        DisplayTextThenFadeOut(_postRaidTextController);
        OnRaidComplete?.Invoke();
    }

    private void TransitionToRaidFailedState()
    {
        _currentState = RaidState.RaidFailed;
        OnRaidFailed?.Invoke();
    }


    private WaveConfig currentWave => raidConfig.waves[_currentWaveIndex];
    private bool allActiveEnemiesAreDead => !_activeEnemies.Any(enemy => enemy != null);
    private bool allCurrentWaveEnemiesSpawned => _activeEnemies.Count == currentWave.enemies.Sum(enemyData => enemyData.count);
    


    private void StartNextWave()
    {
        if (_currentWaveIndex >= 0) 
        { 
            AwardPrizesForWave(currentWave);
        }
        _currentWaveIndex += 1;
        StartTimer();
        DisplayTextThenFadeOut(_alertTextController);
    }

    private void AwardPrizesForWave(WaveConfig wave)
    {
        if (wave.wavePrizes == null || wave.wavePrizes.Count == 0)
        {
            return; // No prizes for this wave
        }

        int prizesCount = 0;

        foreach (var prize in wave.wavePrizes)
        {
            if (prize.itemPrefab == null) continue;

            IInventoryItem itemData = prize.itemPrefab.GetComponent<IInventoryItem>();
            if (itemData != null)
            {
                InventoryManager.Instance.AddItem(itemData, prize.quantity);
                prizesCount += prize.quantity;
            }
            else
            {
                Debug.LogWarning($"Prize prefab {prize.itemPrefab.name} is missing an IInventoryItem component!");
            }
        }

        _prizePopupTextController.SetText($"You have earned {prizesCount} prizes!");
        DisplayTextThenFadeOut(_prizePopupTextController);
    }

    private void StartTimer()
    {
        _timeBeforeNextWaveInSeconds = currentWave.countdown;
        _timerTextController.SetText(Utils.FormatTime(_timeBeforeNextWaveInSeconds));
        _timerTextController.SetTextVisible();
        StartCoroutine(Utils.ExecuteFunctionAfterDelay(_timeBeforeNextWaveInSeconds + 0.5f, _timerTextController.SetTextInvisible));
    }

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

    private void UpdateEnemiesCountText() => _enemiesRemainingTextController.SetText($"Enemies Remaining: {_activeEnemies.Count(e => e != null)}");

    private void RemoveEnemiesCountText() => StartCoroutine(_enemiesRemainingTextController.FadeOut(1f));

    private IEnumerator SpawnWaveEnemiesOverIntervals()
    {
        if (spawnPoints.Count == 0)
            Debug.LogError("No spawn points assigned for raid wave.");

        List<EnemyData> enemiesToSpawn = new List<EnemyData>();

        foreach (var enemyData in currentWave.enemies)
            enemiesToSpawn.Add(new EnemyData { prefab = enemyData.prefab, count = enemyData.count, });

        do
        {
            List<EnemyData> availableEnemies = enemiesToSpawn.FindAll(enemy => enemy.count > 0);
            if (availableEnemies.Count == 0)
                break;

            EnemyData chosenEnemyData = availableEnemies[Random.Range(0, availableEnemies.Count)];

            Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject newEnemy = Instantiate(chosenEnemyData.prefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);

            _activeEnemies.Add(newEnemy);

            chosenEnemyData.count--;

            yield return new WaitForSeconds(currentWave.spawnInterval);
        } while (true);
    }
}

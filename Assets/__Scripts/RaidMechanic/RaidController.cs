using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class RaidController : MonoBehaviour
{

    // ------- TEXT -------
    [SerializeField]
    private TextMeshProUGUI alertText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    private TextController _alertTextController;
    private TextController _timerTextController;

    // ------- TIMERS -------
    private float _timeBeforeNextWaveInSeconds = 0;

    private float _tickTimer = 0f;

    // ------- CONFIG & STATE DATA -------
    [SerializeField]
    private RaidConfig raidConfig;
    [SerializeField]
    private List<Transform> spawnPoints;
    private int _currentWaveIndex;
    private List<GameObject> _activeEnemies = new List<GameObject>();
    public enum RaidState { PreRaid, PreWave, WaveInProgress, RaidComplete }
    private RaidState _currentState;


    void Awake()
    {
        _alertTextController = new TextController(alertText);
        _timerTextController = new TextController(timerText);
        TransitionToState(RaidState.PreRaid);
    }

    void Update()
    {

        if (_currentState == RaidState.PreWave)
        {
            UpdateTimer();
            if (_timeBeforeNextWaveInSeconds <= 0)
                TransitionToState(RaidState.WaveInProgress);

        }
        else if (_currentState == RaidState.WaveInProgress)
        {
            if (allCurrentWaveEnemiesSpawned && allActiveEnemiesAreDead)
            {
                if (_currentWaveIndex + 1 < raidConfig.waves.Count)
                    TransitionToState(RaidState.PreWave);
                else
                    TransitionToState(RaidState.RaidComplete);
                _activeEnemies.Clear();
            }
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
    
    private void TransitionToState(RaidState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case RaidState.PreRaid:
                _currentState = RaidState.PreRaid;
                _currentWaveIndex = -1;
                break;

            case RaidState.PreWave:
                StartNextWave();
                _alertTextController.SetText("Enemy raid observed in the distance! Prepare to fight!");
                DisplayAlertText();
                break;

            case RaidState.WaveInProgress:
                _alertTextController.SetText("The enemies are here! Attack!!!");
                DisplayAlertText();
                StartCoroutine(SpawnWaveEnemiesOverIntervals());
                break;

            case RaidState.RaidComplete:
                _alertTextController.SetText("Raid defended successfully!");
                break;  
        }
    }
    
    private bool allActiveEnemiesAreDead => !_activeEnemies.Any(enemy => enemy != null);
    private bool allCurrentWaveEnemiesSpawned => _activeEnemies.Count == raidConfig.waves[_currentWaveIndex].enemies.Sum(enemyData => enemyData.count);
    

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && _currentState == RaidState.PreRaid)
        {
            TransitionToState(RaidState.PreWave);
        }
    }

    private void StartNextWave()
    {
        _currentWaveIndex += 1;
        StartTimer();
        DisplayAlertText();
    }

    private void StartTimer()
    {
        _timeBeforeNextWaveInSeconds = raidConfig.waves[_currentWaveIndex].countdown;
        _timerTextController.SetText(Utils.FormatTime(_timeBeforeNextWaveInSeconds));
        _timerTextController.SetTextVisible();
        StartCoroutine(Utils.ExecuteCoroutineAfterDelay(_timeBeforeNextWaveInSeconds + 1, _timerTextController.SetTextInvisible));
    }

    private void DisplayAlertText()
    {
        _alertTextController.SetTextVisible();
        StartCoroutine(Utils.ExecuteFunctionAfterDelay(2f, _alertTextController.FadeOut(1f), this));
    }

    private IEnumerator SpawnWaveEnemiesOverIntervals()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned for raid wave.");
        }

        List<EnemyData> enemiesToSpawn = new List<EnemyData>();

        var currentWave = raidConfig.waves[_currentWaveIndex];
        foreach (var enemyData in currentWave.enemies)
        {
            enemiesToSpawn.Add(new EnemyData { prefab = enemyData.prefab, count = enemyData.count, });
        }


        do
        {
            List<EnemyData> availableEnemies = enemiesToSpawn.FindAll(enemy => enemy.count > 0);
            if (availableEnemies.Count == 0)
            {
                break;
            }

            EnemyData chosenEnemyData = availableEnemies[Random.Range(0, availableEnemies.Count)];

            Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject newEnemy = Instantiate(chosenEnemyData.prefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
            Debug.Log($"Spawned the enemy of index {_activeEnemies.Count}");
            _activeEnemies.Add(newEnemy);

            chosenEnemyData.count--;

            yield return new WaitForSeconds(currentWave.spawnInterval);
        } while (true);

    }


}

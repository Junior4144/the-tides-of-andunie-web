using UnityEngine;

public class CannonSpawner : MonoBehaviour
{


    [SerializeField]
    private GameObject _cannonBallPrefab;

    [SerializeField]
    private float _miniumSpawnTime;
    [SerializeField]
    private float _maxiumSpawnTime;

    private float _timeUntillSpawn;

    void Awake()
    {
        SetTimeUntillSpawn();
    }

    void Update()
    {
        _timeUntillSpawn -= Time.deltaTime;

        if (_timeUntillSpawn <= 0)
        {
            Instantiate(_cannonBallPrefab, transform.position, Quaternion.identity);
            SetTimeUntillSpawn();
        }

    }

    private void SetTimeUntillSpawn()
    {
        _timeUntillSpawn = Random.Range(_miniumSpawnTime, _maxiumSpawnTime);
    }

}
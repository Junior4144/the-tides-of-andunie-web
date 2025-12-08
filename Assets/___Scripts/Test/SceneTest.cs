using UnityEngine;

public class SceneTest : MonoBehaviour
{
    private float _updateInterval = 3f;
    private float _nextUpdateTime;

    private string _prefix;

    void Awake()
    {
        _prefix = $"[{GetType().Name}]";
        Debug.Log($"{_prefix} Awake");
    }

    void OnEnable()
    {
        Debug.Log($"{_prefix} OnEnable");
    }

    void Start()
    {
        Debug.Log($"{_prefix} Start");
        _nextUpdateTime = Time.time + _updateInterval;
    }

    void Update()
    {
        if (Time.time >= _nextUpdateTime)
        {
            Debug.Log($"{_prefix} Update (3s interval)");
            _nextUpdateTime = Time.time + _updateInterval;
        }
    }
}
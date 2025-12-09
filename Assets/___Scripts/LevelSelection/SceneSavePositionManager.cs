using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSavePositionManager : MonoBehaviour
{
    private readonly Dictionary<string, (Vector3 pos, Quaternion rot)> _playerPrevPosition = new();

    public static SceneSavePositionManager Instance { get; private set; }

    private string _lastSceneName;
    public string LastSceneName => _lastSceneName;

    private Vector3 _lastPosition;
    public Vector3 LastPosition => _lastPosition;

    private Quaternion _lastRotation;
    public Quaternion LastRotation => _lastRotation;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SavePlayerPosition(string sceneName, Vector3 position, Quaternion rotation)
    {
        Debug.Log($"[SceneSavePositionManager] Position saved {sceneName} {position}");
        _playerPrevPosition[sceneName] = (position, rotation);

        _lastSceneName = sceneName;
        _lastPosition = position;
        _lastRotation = rotation;
    }

    public void SaveLastScene(string sceneName)
    {
        _lastSceneName = sceneName;
    }

    public void ResetPlayerPosition(string sceneName)
    {
        Debug.Log($"[SceneSavePositionManager] Position reset {sceneName}");
        _playerPrevPosition.Remove(sceneName);
    }

    public (Vector3 pos, Quaternion rot)? GetSavedPosition(string sceneName)
    {
        PrintDictionary();
        if (_playerPrevPosition.ContainsKey(sceneName))
        {
            Debug.Log($"[SceneSavePositionManager] Position found {sceneName}");
            return _playerPrevPosition[sceneName];
        }
        Debug.Log($"[SceneSavePositionManager] Position not found {sceneName}");
        return null;
    }

    public void PrintDictionary()
    {
        Debug.Log("===Saved Positions===");
        foreach (var kvp in _playerPrevPosition)
        {
            var (pos, rot) = kvp.Value;
            Debug.Log($"[SceneSavePositionManager] Scene {kvp.Key} pos:{pos} rot:{rot}");
        }
    }

    public void ResetSceneSaveData()
    {
        _playerPrevPosition.Clear();

        _lastSceneName = SceneManager.GetActiveScene().name;
        _lastPosition = Vector3.zero;
        _lastRotation = Quaternion.identity;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SceneSavePositionManager : MonoBehaviour
{
    private readonly Dictionary<string, (Vector3 pos, Quaternion rot)> _playerPrevPosition = new();

    public static SceneSavePositionManager Instance { get; private set; }

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
        Debug.Log($"Saving Player Last Position in {sceneName} at {position} and {rotation}");

        _playerPrevPosition[sceneName] = (position, rotation);

    }
    public void ResetPlayerPosition(string sceneName)
    {
        _playerPrevPosition.Remove(sceneName);
    }

    public (Vector3 pos, Quaternion rot)? GetSavedPosition(string sceneName)
    {
        Debug.Log($"Trying to get from dictionary at {sceneName}");
        PrintDic();
        if ( _playerPrevPosition.ContainsKey(sceneName))
            return _playerPrevPosition[sceneName];
        return null;
    }

    public void PrintDic()
    {
        foreach (var kvp in _playerPrevPosition)
        {
            var (pos, rot) = kvp.Value;
            Debug.Log($"Scene: {kvp.Key} | Position: {pos} | Rotation: {rot}");
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance { get; private set; }

    [SerializeField] private string persistentSceneName = "PersistentGameplay";

    public static event Action OnNonPersistentSceneActivated;  // fires only for gameplay scenes

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SceneManager.activeSceneChanged += HandleActiveSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
    }

    private void HandleActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log($"[SceneStateManager] Active scene changed from {oldScene.name} to {newScene.name}");

        if (newScene.name != persistentSceneName)
        {
            Debug.Log($"[SceneStateManager] Non-persistent scene {newScene.name} activated.");
            OnNonPersistentSceneActivated?.Invoke();
        }
    }
}

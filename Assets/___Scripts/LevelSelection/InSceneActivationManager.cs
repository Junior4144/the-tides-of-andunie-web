using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InSceneActivationManager : MonoBehaviour
{
    public static event Action OnSceneActivated;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene == gameObject.scene)
        {
            Debug.Log($"Scene {newScene.name} just became active — sending event.");
            OnSceneActivated?.Invoke();
        }
    }
}

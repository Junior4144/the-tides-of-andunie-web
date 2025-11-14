using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateSetter : MonoBehaviour
{
    [SerializeField]
    private GameState stateToSet;

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SetState(stateToSet);
        }

    }

}


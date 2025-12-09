using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village9LSController : MonoBehaviour
{
    private BoxCollider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<BoxCollider2D>();
    }

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
            HandleSetup();
    }

    private void HandleSetup()
    {
        if(LSManager.Instance.HasInvasionStarted)
        {
            _collider2D.enabled = true;
        }
        else
        {
            _collider2D.enabled = false;    
        }
    }
}

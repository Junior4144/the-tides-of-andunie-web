using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private float _sceneFadeDuration;

    private SceneFade _sceneFade;

    private void Awake() =>
        _sceneFade = GetComponentInChildren<SceneFade>();

    private IEnumerator Start()
    {
        yield return _sceneFade.FadeInCoroutine(_sceneFadeDuration);
    }

    public void LoadScene(string senseName) =>
        StartCoroutine(LoadSceneCoroutine(senseName));

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return _sceneFade.FadeOutCoroutine(_sceneFadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}

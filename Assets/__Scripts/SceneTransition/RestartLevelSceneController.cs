using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelSceneController : MonoBehaviour
{
    //on invoke by player death
    //will restart level with fade animation
    public static RestartLevelSceneController Instance { get; private set; }

    [SerializeField] private float _sceneFadeDuration = 1f;
    private SceneFade _sceneFade;

    public float timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    private IEnumerator Start()
    {
        yield return _sceneFade.FadeInCoroutine(_sceneFadeDuration);
    }

    // 🧩 New method that checks if PersistentGameplay is already loaded
    public void LoadNextStage(string sceneToBeUnloaded)
    {
        StartCoroutine(LoadNextStageCoroutine(sceneToBeUnloaded));
    }
    //sceneToBeUnloaded
    private IEnumerator LoadNextStageCoroutine(string additiveScene)
    {
        Debug.Log("Restart Scene Starting");
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));

        Debug.Log($"Unloading old scene: {additiveScene}");
        yield return SceneManager.UnloadSceneAsync(additiveScene);

        Debug.Log($"Loading next additive scene: {additiveScene}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
            yield return null;

        // 5. Set the newly loaded scene as active
        Scene nextScene = SceneManager.GetSceneByName(additiveScene);
        SceneManager.SetActiveScene(nextScene);
        Debug.Log($"Active scene set to: {nextScene.name}");

        // 7. Fade back in
        yield return StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));

        Debug.Log($"Finished loading {additiveScene}");



    }
    private IEnumerator LoadNextStageCoroutine(string sceneToBeUnloaded, string additiveScene)
    {
        Debug.Log("Next Scene Change Starting");
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));

        GameObject _player = PlayerManager.Instance.gameObject;
        if (_player != null)
            SaveManager.Instance.SavePlayerStats();

        Debug.Log($"UnloadSceneAsync: {sceneToBeUnloaded}");
        yield return SceneManager.UnloadSceneAsync(sceneToBeUnloaded); // might be making persistent scene active

        Debug.Log($"Loading next additive scene: {additiveScene}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
            yield return null;


        Scene nextScene = SceneManager.GetSceneByName(additiveScene);
        SceneManager.SetActiveScene(nextScene);
        Debug.Log($"Sets {additiveScene} as Active Scene");

        yield return StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));

        Debug.Log("Completed Scene Change");
    }


}

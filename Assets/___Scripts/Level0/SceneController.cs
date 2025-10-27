using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
public class SceneController : MonoBehaviour
{
    [SerializeField] private float _sceneFadeDuration = 1f;
    private SceneFade _sceneFade;

    public float timer = 0f;

    private void Awake()
    {
        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    private IEnumerator Start()
    {
        yield return _sceneFade.FadeInCoroutine(_sceneFadeDuration);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return _sceneFade.FadeOutCoroutine(_sceneFadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    //New method that checks if PersistentGameplay is already loaded
    public void LoadNextStage(string persistentScene, string sceneToBeUnloaded, string additiveScene)
    {
        StartCoroutine(LoadNextStageCoroutine(persistentScene, sceneToBeUnloaded, additiveScene));
    }

    private IEnumerator LoadNextStageCoroutine(string persistentScene, string sceneToBeUnloaded, string additiveScene)
    {
        // Fade out first
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));


        //To be removed if possible
        // Ensure PersistentGameplay is loaded once
        //Scene persistent = SceneManager.GetSceneByName(persistentScene);
        //if (!persistent.isLoaded)
        //{
        //    Debug.Log("Loading Persistent Scene...");
        //    yield return SceneManager.LoadSceneAsync(persistentScene, LoadSceneMode.Additive);
        //}

        Debug.Log("Next Scene Change Starting");
        Debug.Log($"UnloadSceneAsync: {sceneToBeUnloaded}");
        yield return SceneManager.UnloadSceneAsync(sceneToBeUnloaded);


        Debug.Log($"Loading next additive scene: {additiveScene}");
        yield return SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);


        // Set the newly loaded scene as active
        Scene nextScene = SceneManager.GetSceneByName(additiveScene);
        SceneManager.SetActiveScene(nextScene);
        Debug.Log($"nextScene.SetActiveScene: {nextScene}");


        // Fade in after the scene is ready
        yield return StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));

        Debug.Log("Am i still running");
    }




    //private void LoadNextStageCoroutine(string persistentScene, string sceneToBeUnloaded, string additiveScene)
    //{
    //    StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));

    //    // Ensure PersistentGameplay is loaded once
    //    Scene persistent = SceneManager.GetSceneByName(persistentScene);
    //    if (!persistent.isLoaded)
    //    {
    //        Debug.Log("Loading Persistent Scene...");
    //        SceneManager.LoadSceneAsync(persistentScene, LoadSceneMode.Additive);
    //    }

    //    Debug.Log($"UnloadSceneAsync: {sceneToBeUnloaded}");
    //    SceneManager.UnloadSceneAsync(sceneToBeUnloaded);



    //    // Load the next stage additively
    //    Debug.Log($"Loading next additive scene: {additiveScene}");
    //    SceneManager.LoadScene(additiveScene, LoadSceneMode.Additive);


    //    // Make sure the additive scene is active (important for lighting & instantiation)
    //    Scene nextScene = SceneManager.GetSceneByName(additiveScene);
    //    SceneManager.SetActiveScene(nextScene);




    //    StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));
    //}


}

using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ReloadPersistentController : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(HandleSetup());
    }

    private IEnumerator HandleSetup()
    {
        yield return new WaitForSeconds(1f);
        PersistentSceneManager.ReloadPersistentAsync();
        yield return new WaitForSeconds(1f);
        LoadNextStage();
    }
    private void LoadNextStage()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneControllerManager.Instance.LoadNextStage(currentScene, "Main Menu");
    }
}
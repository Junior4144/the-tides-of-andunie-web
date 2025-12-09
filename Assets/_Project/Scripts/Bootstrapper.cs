using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public static class PersistentSceneManager
{
    public static AsyncOperationHandle<SceneInstance>? PersistentHandle;
    
    public static IEnumerator ReloadPersistentAsync()
    {
        if (PersistentHandle.HasValue)
        {
            var unloadOp = Addressables.UnloadSceneAsync(PersistentHandle.Value);
            yield return unloadOp;
            PersistentHandle = null;
        }
        
        var loadOp = Addressables.LoadSceneAsync(
            "PersistentGameplay",
            LoadSceneMode.Additive,
            activateOnLoad: false
        );

        PersistentHandle = loadOp;
        yield return loadOp;
        
        yield return loadOp.Result.ActivateAsync();
    }
}

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Debug.Log("Starting Load Persistent Scene Automatically");

        if (IsSceneLoaded("PersistentGameplay"))
        {
        #if UNITY_EDITOR
            Debug.Log("[Bootstrapper] PersistentGameplay scene already loaded skipping bootstrap load.");
        #endif
            return;
        }

        var handle = Addressables.LoadSceneAsync("PersistentGameplay", LoadSceneMode.Additive);
        handle.WaitForCompletion();

        PersistentSceneManager.PersistentHandle = handle;
    }
    private static bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}

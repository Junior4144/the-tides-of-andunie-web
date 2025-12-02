using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ReloadPersistentController : MonoBehaviour
{
    private AsyncOperationHandle<SceneInstance>? loadedHandle = null;

    void Start()
    {
        if (IsSceneLoaded("PersistentGameplay"))
        {
#if UNITY_EDITOR
            Debug.Log("[Bootstrapper] PersistentGameplay scene already loaded — unloading it first.");
#endif
            UnloadPersistent();
        }

        LoadPersistent();
    }

    private void LoadPersistent()
    {
        loadedHandle = Addressables.LoadSceneAsync("PersistentGameplay", LoadSceneMode.Additive);
        loadedHandle.Value.WaitForCompletion();
    }

    private void UnloadPersistent()
    {
        // If you kept the original load handle
        if (loadedHandle.HasValue)
        {
            Addressables.UnloadSceneAsync(loadedHandle.Value);
        }
        else
        {
            // fallback: unload by name if already in scene list
            Scene scene = SceneManager.GetSceneByName("PersistentGameplay");
            if (scene.IsValid())
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
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
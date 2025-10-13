using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Debug.Log("Load Persistent Scene Automatically");

        if (IsSceneLoaded("PersistentGameplay"))
        {
        #if UNITY_EDITOR
            Debug.Log("[Bootstrapper] PersistentGameplay scene already loaded — skipping bootstrap load.");
        #endif
            return;
        }

        var handle = Addressables.LoadSceneAsync("PersistentGameplay", LoadSceneMode.Additive);
        handle.WaitForCompletion();
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

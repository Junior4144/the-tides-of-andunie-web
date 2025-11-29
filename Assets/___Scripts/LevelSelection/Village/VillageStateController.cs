using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageStateController : MonoBehaviour
{
    [Header("Objects for States")]
    [SerializeField] private GameObject preInvasionObjects;
    [SerializeField] private GameObject invadedObjects;
    [SerializeField] private GameObject liberatedObjects;

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
            StartCoroutine(HandleVillageStateChanged());
    }

    private IEnumerator HandleVillageStateChanged()
    {
        yield return null;
        ApplyCurrentState();
    }

    private void ApplyCurrentState()
    {
        VillageState state = LSManager.Instance.GetVillageState(VillageIDManager.Instance.villageId);
        ApplyState(state);
    }


    private void ApplyState(VillageState state)
    {
        switch (state)
        {
            case VillageState.PreInvasion:
                SetPreInvasionState();
                break;

            case VillageState.Invaded:
                SetInvadedState();
                break;

            case VillageState.Liberated_FirstTime:
            case VillageState.Liberated_Done:
                SetLiberatedState();
                break;
        }
    }

    private void SetPreInvasionState()
    {
        preInvasionObjects?.SetActive(true);
        invadedObjects?.SetActive(false);
        liberatedObjects?.SetActive(false);
    }

    private void SetInvadedState()
    {
        preInvasionObjects?.SetActive(false);
        invadedObjects?.SetActive(true);
        liberatedObjects?.SetActive(false);
    }

    private void SetLiberatedState()
    {
        preInvasionObjects?.SetActive(false);
        invadedObjects?.SetActive(false);
        liberatedObjects?.SetActive(true);
    }
}

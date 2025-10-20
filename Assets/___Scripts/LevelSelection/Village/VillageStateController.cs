using System.Collections;
using UnityEngine;

public class VillageStateController : MonoBehaviour
{
    [Header("Village Identifier")]
    [SerializeField] private string villageId;

    [Header("Objects for States")]
    [SerializeField] private GameObject preInvasionObjects;
    [SerializeField] private GameObject invadedObjects;
    [SerializeField] private GameObject liberatedObjects;

    private void OnEnable() => LSManager.Instance.OnVillageStateChanged += HandleVillageStateChanged;

    private void OnDisable() => LSManager.Instance.OnVillageStateChanged -= HandleVillageStateChanged;

    private void Start() => StartCoroutine(HandleVillageStateChanged());

    private IEnumerator HandleVillageStateChanged()
    {
        yield return null;
        ApplyCurrentState(); // Hydrate when scene loads
    }

    private void ApplyCurrentState()
    {
        VillageState state = LSManager.Instance.GetVillageState(villageId);
        ApplyState(state);
    }

    private void HandleVillageStateChanged(string id, VillageState newState)
    {
        if (id != villageId)
            return;

        ApplyState(newState);
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

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableWeaponController : MonoBehaviour
{
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
            HandleDisablingOfWeapon();
    }

    private void HandleDisablingOfWeapon()
    {
        string villageId = VillageIDManager.Instance.villageId;
        VillageState villageState = LSManager.Instance.GetVillageState(villageId);

        if (villageState != VillageState.Invaded)
        {
            WeaponManager.Instance.DisableWeapon();
        }
    }
}

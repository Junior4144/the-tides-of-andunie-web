using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LiberationUIController : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject UnlockedRegionUI;
    public GameObject UnlockedRegionUISecondPart;

    public TextMeshProUGUI VillageNameText;

    public TextMeshProUGUI RegionNameText;

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
            StartCoroutine(HandleLiberationLogic());
    }

    private IEnumerator HandleLiberationLogic()
    {
        if (GlobalStoryManager.Instance.HasExitedLiberation)
        {
            //LIBERATION SECTION
            yield return new WaitForSeconds(.5f);
            targetObject.SetActive(true);
            UIEvents.DefaultPopUPActive?.Invoke();

            string id = GlobalStoryManager.Instance.LastLiberatedVillageID;
            VillageNameText.text = LSManager.Instance.GetVillageName(id);

            GlobalStoryManager.Instance.SetBool("playLSInvasionCutscene", false);

            // REGION SECTION
            yield return new WaitForSeconds(.2f);

            Region unlockedRegion = LSRegionLockManager.Instance.CheckForNewUnlockedRegion();
            if (unlockedRegion != Region.None)
            {
                Debug.Log("New region unlocked: " + unlockedRegion);
                UnlockedRegionUI.SetActive(true);
                UnlockedRegionUISecondPart.SetActive(true);
                RegionNameText.text = unlockedRegion.ToString();
            }
        }
    }

    public void OnButtonClicked()
    {
        if (targetObject != null)
        {
            UIEvents.DefaultPopUpDisabled.Invoke();
            UnlockedRegionUI.SetActive(false);
            UnlockedRegionUISecondPart.SetActive(false);
            targetObject.SetActive(false);
        }
    }
}
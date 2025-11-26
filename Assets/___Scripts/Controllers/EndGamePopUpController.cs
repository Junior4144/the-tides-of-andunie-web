using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePopUpController : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return new WaitForSeconds(1f);

        if (newScene == gameObject.scene)
            HandleSetup();
    }

    private void HandleSetup()
    {
        if (int.Parse(LSManager.Instance.GetLiberatedVillageAmount()) ==
            (int)LSManager.Instance.GetTotalPlayableVillage())
        {
            panel.SetActive(true);
            UIEvents.EndGamePopUPActive.Invoke();
        }

    }

    public void HandleButtonClick()
    {
        Application.Quit();
    }
}

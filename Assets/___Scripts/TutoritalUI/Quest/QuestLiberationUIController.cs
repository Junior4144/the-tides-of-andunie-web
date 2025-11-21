using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestLiberationUIController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        canvas.SetActive(false);
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
        SceneManager.activeSceneChanged += HandleCheck;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
        SceneManager.activeSceneChanged -= HandleCheck;
    }

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            HandleQuestUI();

    }
    private void HandleQuestUI()
    {
        HandleDisplayingPanel();
        HandleTextChange();
    }

    private void HandleOnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Cutscene)
        {
            canvas.SetActive(false);
        }
        else if (gameState == GameState.LevelSelector)
        {
            canvas.SetActive(true);
        }
    }

    private void HandleTextChange()
    {
        if (GlobalStoryManager.Instance.HasTalkedToChief == true)
            text.text = $"Liberated Villages: {LSManager.Instance.GetLiberatedVillageAmount()} / {LSManager.Instance.GetTotalPlayableVillage()}";
    }
   
    private void HandleDisplayingPanel()
    {
        if(GlobalStoryManager.Instance.HasTalkedToChief == true)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}

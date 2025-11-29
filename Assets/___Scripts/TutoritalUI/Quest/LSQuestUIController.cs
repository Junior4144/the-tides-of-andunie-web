using UnityEngine;

public class LSQuestUIController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject Panel1;

    private void Awake()
    {
        Panel1.SetActive(true);
    }

    private void Start()
    {
        HandleDisplayingPanel();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
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

    private void HandleDisplayingPanel()
    {
        if (GlobalStoryManager.Instance.HasTalkedToChief == false)
        {
            Panel1.SetActive(true);
        }
        else Panel1.SetActive(false);
    }

}

using UnityEngine;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject Panel1;
    [SerializeField] private GameObject Panel2;

    private void Awake()
    {
        Panel1.SetActive(true);
        Panel2.SetActive(false);
    }

    private void OnEnable()
    {
        RaidController.OnRaidTriggered += HandleRaidTriggered;
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    }

    private void OnDisable()
    {
        RaidController.OnRaidTriggered -= HandleRaidTriggered;
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    }

    private void HandleOnGameStateChanged(GameState gameState)
    {
        if(gameState == GameState.Cutscene)
        {
            canvas.SetActive(false);
        }
        else if(gameState == GameState.Gameplay){
            canvas.SetActive(true);
        }
    }

    private void HandleRaidTriggered()
    {
        Panel1.SetActive(false);

        Panel2.SetActive(true);
    }
}

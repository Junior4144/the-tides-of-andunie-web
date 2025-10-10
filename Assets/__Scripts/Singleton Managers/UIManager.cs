using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Tooltip("The root gameplay canvas (health bar, etc.)")]
    public GameObject gameplayCanvas;

    [Header("UI Groups")]
    [SerializeField] private GameObject gameplayUI;

    private void Awake()
    {

        if (Instance != null) return;

        Instance = this;
        HideAll();
    }

    public void ShowUI(UIActivation.UIType type)
    {
        HideAll();
        Debug.Log($"Current Scene Type: {type} ");
        switch (type)
        {
            case UIActivation.UIType.Gameplay:
                gameplayUI.SetActive(true);
                break;
            case UIActivation.UIType.None:
            default:
                break;
        }
    }
    private void HideAll()
    {
        gameplayUI.SetActive(false);
    }
}

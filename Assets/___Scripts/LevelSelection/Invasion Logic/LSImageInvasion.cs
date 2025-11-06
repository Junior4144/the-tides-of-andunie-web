using UnityEngine;
using UnityEngine.UI;

public class LSImageInvasion : MonoBehaviour
{
    [Header("Village Settings")]
    [SerializeField] private string villageId;

    private Image uiImage;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        LSManager.UpdateVillageInvasionStatus += HandleInvasion;
    }
    private void OnDisable()
    {
        LSManager.UpdateVillageInvasionStatus -= HandleInvasion;
    }

    private void HandleInvasion()
    {
        if (!gameObject.scene.name.Contains("LevelSelector"))
            return;

        if (string.IsNullOrEmpty(villageId))
            return;

        if (LSManager.Instance.GetVillageState(villageId) == VillageState.Invaded)
            HandleInvasionLogic();
        else
        {
            HandleNonInvasionLogic();
        }


    }

    private void HandleInvasionLogic()
    {
        Color c = uiImage.color;
        c.a = 1f;
        uiImage.color = c;

    }
    private void HandleNonInvasionLogic()
    {
        Color c = uiImage.color;
        c.a = 0f;
        uiImage.color = c;

    }
}

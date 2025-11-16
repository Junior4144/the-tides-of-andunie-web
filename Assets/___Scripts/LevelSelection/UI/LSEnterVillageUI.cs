using TMPro;
using UnityEngine;

public class LSEnterVillageUI : MonoBehaviour
{
    //public static LSEnterVillageUI Instance;

    [SerializeField] private GameObject UIPanel;
    [SerializeField] private TMP_Text LevelSelectionEnterHeader;
    [SerializeField] private TMP_Text LSButtonText;

    private bool Clicked = false;
    public bool isActiveUI = false;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //}

    private void OnEnable()
    {
        LSUIManager.ActivateEnterVillageUI += HandleUIActivation;
        LSUIManager.DeactivateEnterVillageUI += HandleUIDeactivation;

        LSUIManager.ActivateVillageExitUI += HandleExitUI;
    }
    
    private void OnDisable()
    {
        LSUIManager.ActivateEnterVillageUI -= HandleUIActivation;
        LSUIManager.DeactivateEnterVillageUI -= HandleUIDeactivation;

        LSUIManager.ActivateVillageExitUI -= HandleExitUI;
    }

    private void HandleUIActivation()
    {
        LevelSelectionEnterHeader.text = "Visit Village";
        UIPanel.SetActive(true);

        if (isActiveUI)
        {
            HandleEnterVillage();
        }
        isActiveUI = true;
    }

    private void HandleExitUI()
    {
        LevelSelectionEnterHeader.text = "Leave Village";
        LSButtonText.text = "Leave";

        UIPanel.SetActive(true);

        if (isActiveUI)
        {
            HandleEnterVillage();
        }   
        isActiveUI = true;
    }

    private void HandleUIDeactivation()
    {
        UIPanel.SetActive(false);
        isActiveUI = false;
    }

    public void HandleEnterVillage()
    {
        Debug.Log("[LSEnterVillageUI] Trying to HandleEnterVillage");
        if (Clicked) return;

        Clicked = true;
        Debug.Log("[LevelSelectionMenu] Button Clicked, success in LSEnterVillageUI");

        LSUIManager.Instance.ButtonClicked();
    }
}

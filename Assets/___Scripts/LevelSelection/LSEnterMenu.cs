using System;
using UnityEngine;

public class LSEnterMenu : MonoBehaviour
{
    //public static LSEnterMenu Instance;

    [SerializeField]
    private GameObject Panel;

    private bool Clicked = false;
    public bool isActive = false;

    public static event Action PreScreenUIActivation;
    public static event Action PreScreenUIDeactivation;

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
        LSUIManager.ActivateEntryUI += HandleUIToggling;
        LSUIManager.DeactivatePreEntryUI += HandleUIDeactivation;
    }
    private void OnDisable()
    {
        LSUIManager.ActivateEntryUI -= HandleUIToggling;
        LSUIManager.DeactivatePreEntryUI -= HandleUIDeactivation;
    }

    private void Start() => Panel.SetActive(false);

    private void HandleUIToggling()
    {
        Panel.SetActive(true);
        isActive = true;
        PreScreenUIActivation?.Invoke();
    }

    public void HandleUIDeactivation()
    {
        Debug.Log("[LSEnterMenu] HandleUIDeactivation");
        Panel.SetActive(false);
        isActive = false;
        PreScreenUIDeactivation?.Invoke();
    }

    public void HandleEnterVillage()
    {
        Debug.Log("[LSEnterMenu] Trying to HandleEnterVillage");
        if (Clicked) return;

        Clicked = true;
        Debug.Log("[LevelSelectionMenu] Button Clicked, success in HandleEnterVillage");

        LSUIManager.Instance.ButtonClicked();
    }
}

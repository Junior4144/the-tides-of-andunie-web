using System;
using UnityEngine;

public class LSEnterMenu : MonoBehaviour // PRE SCREEN UI
{
    [SerializeField]
    private GameObject Panel;

    private bool Clicked = false;

    [HideInInspector]
    public bool isActive = false;


    private void OnEnable()
    {
        UIEvents.OnPreScreenConfirm += HandleUIToggling;

        UIEvents.OnPreScreenDeactivated += HandleUIDeactivation;
    }
    private void OnDisable()
    {
        UIEvents.OnPreScreenConfirm -= HandleUIToggling;

        UIEvents.OnPreScreenDeactivated -= HandleUIDeactivation;
    }

    private void Start() => Panel.SetActive(false);

    public void CancelButtonClick()
    {
        UIEvents.OnPreScreenDeactivated?.Invoke();
    }

    private void HandleUIToggling()
    {
        Panel.SetActive(true);
        isActive = true;
    }

    public void HandleUIDeactivation()
    {
        Debug.Log("[LSEnterMenu] HandleUIDeactivation");
        Panel.SetActive(false);
        isActive = false;
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

using System;
using TMPro;
using UnityEngine;

public class LSEnterVillageUI : MonoBehaviour
{
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private TMP_Text LevelSelectionEnterHeader;
    [SerializeField] private TMP_Text LSButtonText;

    private bool Clicked = false;
    public bool isActiveUI = false;


    private void OnEnable()
    {
        UIEvents.OnLSEnterConfirm += HandleUIActivation;

        UIEvents.OnLSEnterDeactivated += HandleUIDeactivation;
    }
    
    private void OnDisable()
    {
        UIEvents.OnLSEnterConfirm -= HandleUIActivation;

        UIEvents.OnLSEnterDeactivated -= HandleUIDeactivation;

    }

    private void HandleUIActivation(bool isExit)
    {

        PlayerManager.Instance.DisablePlayerMovement();

        if (isActiveUI) // double click enter 
        {
            HandleEnterVillage();
        }

        if (isExit)
        {
            LevelSelectionEnterHeader.text = "Leave Village";
            LSButtonText.text = "OK";
        } 
        else
        {
            LevelSelectionEnterHeader.text = "Visit Village";
            LSButtonText.text = "Enter";
        }

        UIPanel.SetActive(true);

        isActiveUI = true;
    }


    private void HandleUIDeactivation()
    {
        PlayerManager.Instance.EnablePlayerMovement();

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

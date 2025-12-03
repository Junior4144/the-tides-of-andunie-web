using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
//if invaded will start appearing
// need to get list of other regions need to proceed

public class LockedRegionUI : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text regionText;
    [SerializeField] private TMP_Text middleText;
    [SerializeField] private TMP_Text prerequisiteText;

    [SerializeField] private float clickCooldown = 1f;
    private Region lastRegion;
    private bool isClickOnCooldown = false;

    private void OnEnable()
    {
        OnClickOutline.RegionClicked += HandleRegionClicked;

        RegionZoomController.ZoomBelowThreshold += ZoomBelowThreshold;

        RegionEventBus.OnDisableOfRegionUI += HandleDisablingOfRegionUIWithAnimation;

        UIEvents.OnPreScreenConfirm += HandleDisablingOfRegionUIWithAnimation;

        ShopUIController.ShopActivated += HandleDisablingOfRegionUIWithAnimation;

        UIEvents.OnLSEnterConfirm += HandleDisablingOfRegionUIWithAnimation;
    }

    private void OnDisable()
    {
        OnClickOutline.RegionClicked -= HandleRegionClicked;
        RegionZoomController.ZoomBelowThreshold -= ZoomBelowThreshold;

        RegionEventBus.OnDisableOfRegionUI -= HandleDisablingOfRegionUIWithAnimation;

        UIEvents.OnPreScreenConfirm -= HandleDisablingOfRegionUIWithAnimation;

        ShopUIController.ShopActivated -= HandleDisablingOfRegionUIWithAnimation;

        UIEvents.OnLSEnterConfirm -= HandleDisablingOfRegionUIWithAnimation;
    }
    private void HandleDisablingOfRegionUIWithAnimation()
    {
        panel.SetActive(false);
    }

    private void HandleDisablingOfRegionUIWithAnimation(bool isExit)
    {
        panel.SetActive(false);
    }

    private void ZoomBelowThreshold()
    {
        var scaler = panel.GetComponent<ScaleOnEnable>();

        if (scaler != null && scaler.IsAnimating)
            return;

        if (panel.activeSelf)
        {
            if (scaler != null)
            {
                scaler.HideWithScale();
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    private void HandleRegionClicked(Region region)
    {
        if (isClickOnCooldown)
            return;

        
        StartCoroutine(ClickCooldownRoutine());

        var scaler = panel.GetComponent<ScaleOnEnable>();

        if (lastRegion == region)
        {
            if (panel.activeSelf)
            {
                if (scaler != null)
                    scaler.HideWithScale();
                else
                    panel.SetActive(false);
            }
            else
            {
                HandlePanelPopulation(region);
                panel.SetActive(true);
            }

            return;
        }

        lastRegion = region;
        HandlePanelPopulation(region);

        if (!panel.activeSelf)
        {
            panel.SetActive(true);
        }
    }
    private IEnumerator ClickCooldownRoutine()
    {
        isClickOnCooldown = true;
        yield return new WaitForSeconds(clickCooldown);
        isClickOnCooldown = false;
    }


    private void HandlePanelPopulation(Region region)
    {
        var regionLocked = LSRegionLockManager.Instance.IsRegionLocked(region);
        regionText.text = regionLocked == false
            ? "Region   Unlocked"
            : "Region   Locked";

        middleText.text = regionLocked == false
            ? ""
            : "You  Must  Liberate  the  following  region";


        var regionList = LSRegionLockManager.Instance.GetPrerequisiteRegions(region);

        // Set prerequisite list label
        prerequisiteText.text =
            regionList.Count == 0
            ? "No  prerequisites. \nRegion  can  be  traveled."
            : string.Join("\n", regionList.Select(r => r.ToString()));

        if (LSRegionLockManager.Instance.IsRegionLocked(region)) return;
        prerequisiteText.text = "No  prerequisites. \nRegion  can  be  traveled.";
    }
}

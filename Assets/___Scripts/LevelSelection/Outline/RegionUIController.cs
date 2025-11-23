using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RegionUIController : MonoBehaviour
{
    [Header("Main UI Elements")]
    [SerializeField] private GameObject RegionCanvas;
    [SerializeField] private GameObject RegionPanel;
    [SerializeField] private float clickCooldown = 1f;
    //Main Title
    private TextMeshProUGUI mainTitleText;
    //Title Panel
    private List<GameObject> villagePanels = new();
    private List<TextMeshProUGUI> villageTitleTexts = new();
    private List<List<GameObject>> villageDifficultyObjects = new();

    //Status Panel
    private List<TextMeshProUGUI> villageStatusTexts = new();
    private List<GameObject> villageStatusImages = new();

    //Reward Panel
    private List<List<GameObject>> villageRewardObjects = new();

    private List<GameObject> villageDiffPanels = new();
    private List<GameObject> villageRewardPanels = new();

    private bool isClickOnCooldown = false;

    private void Awake()
    {
        int childCount = RegionPanel.transform.childCount;

        for (int i = 1; i < childCount; i++)
        {
            mainTitleText = RegionPanel.transform
                .Find("TextPanel/Text (TMP)")
                .GetComponent<TextMeshProUGUI>();


            var panel = RegionPanel.transform.GetChild(i).gameObject;
            villagePanels.Add(panel);

            // ------ Title Text ------
            var text = panel.transform
                .Find("TitlePanel/TextPanel/Text (TMP)")
                .GetComponent<TextMeshProUGUI>();

            villageTitleTexts.Add(text);

            // ------ Skull Panel ------
            var skullPanel = panel.transform.Find("TitlePanel/SkullPanel");
            villageDiffPanels.Add(skullPanel.gameObject);

            List<GameObject> skulls = new();

            for (int j = 0; j < skullPanel.childCount; j++)
            {
                skulls.Add(skullPanel.GetChild(j).gameObject);
            }

            villageDifficultyObjects.Add(skulls);

            // ------ Status Text ------
            var mainStatusImagePanel = panel.transform.Find("StatusPanel/ImagePanel");
            villageStatusImages.Add(mainStatusImagePanel.gameObject);
            

            var statusText = panel.transform
                .Find("StatusPanel/Panel/Text (TMP)")
                .GetComponent<TextMeshProUGUI>();

            villageStatusTexts.Add(statusText);

            // ------ Reward Panel ------
            var rewardPanel = panel.transform.Find("RewardsPanel/ImagePanel");

            List<GameObject> rewards = new();

            for (int k = 0; k < rewardPanel.childCount; k++)
            {
                rewards.Add(rewardPanel.GetChild(k).gameObject);
            }

            villageRewardObjects.Add(rewards);

            var mainRewardPanel = panel.transform.Find("RewardsPanel");
            villageRewardPanels.Add(mainRewardPanel.gameObject);
        }
    }
    private void OnEnable()
    {
        OnClickOutline.RegionClicked += HandleRegionClicked;

        RegionZoomController.ZoomAboveThreshold += ZoomAboveThreshold;
        RegionZoomController.ZoomBelowThreshold += ZoomBelowThreshold;

        RegionZoomController.OnDisableOfRegionUI += HandleDisablingOfRegionUI;
    }

    private void OnDisable()
    {
        OnClickOutline.RegionClicked -= HandleRegionClicked;
        RegionZoomController.ZoomAboveThreshold -= ZoomAboveThreshold;
        RegionZoomController.ZoomBelowThreshold -= ZoomBelowThreshold;

        RegionZoomController.OnDisableOfRegionUI -= HandleDisablingOfRegionUI;
    }

    private void HandleDisablingOfRegionUI()
    {
        RegionPanel.SetActive(false);
    }

    private Region lastRegion;

    private void HandleRegionClicked(Region region)
    {
        if (isClickOnCooldown)
            return;

        StartCoroutine(ClickCooldownRoutine());

        var scaler = RegionPanel.GetComponent<ScaleOnEnable>();

        if (lastRegion == region)
        {
            if (RegionPanel.activeSelf)
            {
                if (scaler != null)
                    scaler.HideWithScale();
                else
                    RegionPanel.SetActive(false);
            }
            else
            {
                HandlePanelPopulation(region);
                RegionPanel.SetActive(true);
            }

            return;
        }

        lastRegion = region;
        HandlePanelPopulation(region);

        if (!RegionPanel.activeSelf)
        {
            RegionPanel.SetActive(true);
        }
    }

    private IEnumerator ClickCooldownRoutine()
    {
        isClickOnCooldown = true;
        yield return new WaitForSeconds(clickCooldown);
        isClickOnCooldown = false;
    }

    private void ZoomBelowThreshold()
    {
        var scaler = RegionPanel.GetComponent<ScaleOnEnable>();

        if (scaler != null && scaler.IsAnimating)
            return;

        if (RegionPanel.activeSelf)
        {
            if (scaler != null)
            {
                scaler.HideWithScale();
            }
            else
            {
                RegionPanel.SetActive(false);
            }
        }
    }

    private void ZoomAboveThreshold()
    {
        RegionCanvas.SetActive(true);
    }

    private void HandlePanelPopulation(Region region)
    {
        var listOfVillages = LSManager.Instance.GetVillagesByRegion(region);

        Debug.Log($"Region: {region}");

        //Resetting panel
        for (int i = 0; i < villagePanels.Count; i++)
        {
            mainTitleText.text = "";

            // Hide the panel entirely
            villagePanels[i].SetActive(false);

            // Clear title
            villageTitleTexts[i].text = "";

            // Turn off all skulls
            foreach (var skull in villageDifficultyObjects[i])
                skull.SetActive(false);

            // Clear status
            villageStatusTexts[i].text = "";

            villageStatusImages[i].SetActive(false);

            // Clear rewards
            foreach (var rewardSlot in villageRewardObjects[i])
            {
                var img = rewardSlot.GetComponent<UnityEngine.UI.Image>();
                img.sprite = null;
            }

            // Hide difficulty + reward panels
            villageDiffPanels[i].SetActive(false);
            villageRewardPanels[i].SetActive(false);
        }

        mainTitleText.text = region.ToString();

        for (int i = 0; i < listOfVillages.Count; i++)
        {

            var villageData = listOfVillages[i];

            villagePanels[i].SetActive(true);

            // Title + difficulty
            villageTitleTexts[i].text = villageData.villageName;
            DisplayDifficultySkulls(i, villageData.diffculty);

            // Status
            villageStatusTexts[i].text = $"Status: {GetVillageStatusText(villageData.state)}";
            villageStatusImages[i].SetActive(villageData.state == VillageState.Invaded);

            // Rewards
            UpdateVillageRewardIcons(i, villageData.rewardConfig);

            bool hideExtraPanels =
                villageData.state == VillageState.PreInvasion ||
                villageData.state == VillageState.Liberated_Done ||
                villageData.state == VillageState.Liberated_FirstTime;

            villageDiffPanels[i].SetActive(!hideExtraPanels);
            villageRewardPanels[i].SetActive(!hideExtraPanels);
        }
    }

    private void DisplayDifficultySkulls(int villageIndex, int difficulty)
    {
        var skulls = villageDifficultyObjects[villageIndex];

        for (int i = 0; i < skulls.Count; i++)
        {
            skulls[i].SetActive(i < difficulty);
        }
    }

    public void UpdateVillageRewardIcons(int villageIndex, RewardsConfig config)
    {
        var rewardSlots = villageRewardObjects[villageIndex];

        if (config == null || config.RewardItems.Count == 0)
        {
            foreach (var slot in rewardSlots)
            {
                var img = slot.GetComponent<UnityEngine.UI.Image>();
                img.sprite = null;
            }
            return;
        }

        var rewards = config.RewardItems;
        for (int i = 0; i < rewards.Count && i < rewardSlots.Count; i++)
        {
            var iconSprite = rewards[i].Item?.SpriteIcon;

            var img = rewardSlots[i].GetComponent<UnityEngine.UI.Image>();
            img.sprite = iconSprite;
        }
    }

    private string GetVillageStatusText(VillageState state)
    {
        return state switch
        {
            VillageState.PreInvasion => "Peaceful",
            VillageState.Invaded => "Invaded",
            VillageState.Liberated_FirstTime => "Liberated",
            VillageState.Liberated_Done => "Liberated",
            _ => "Unknown"
        };
    }
}
using UnityEngine;

public class LSEnterMenu : MonoBehaviour
{
    private GameObject Panel;

    private bool Clicked = false;

    private bool panelIsActive = false;

    private void Awake()
    {
        Panel = GameObject.Find("EntryPanel");
    }

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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return) && Panel.activeInHierarchy)
        //{
        //    if (Clicked) return;

        //    Clicked = true;
        //    Debug.Log("[LevelSelectionMenu] Button Clicked");

        //    LSUIManager.Instance.ButtonClicked();
        //}
    }

    private void HandleUIToggling()
    {
        Debug.Log("[LSEnterMenu] HandleUIToggling");

        Debug.Log("activeSelf: " + Panel.activeSelf);
        Debug.Log("activeInHierarchy: " + Panel.activeInHierarchy);
        Debug.Log("Parent active: " + (Panel.transform.parent?.gameObject.activeInHierarchy));

        Debug.Log($"[LSEnterMenu] HandleUIToggling");
        Debug.Log($"[LSEnterMenu] panelIsActive : {panelIsActive}");

        if (panelIsActive)
        {
            Debug.Log("[LSEnterMenu] Panel already active, HandleEnterVillage");
            HandleEnterVillage();
        }
        else
        {
            panelIsActive = true;
            Debug.Log("[LSEnterMenu] panel not active in Hierarchy, setting to true");
            Panel.SetActive(true);
        }
        
    }

    public void HandleUIDeactivation()
    {
        Debug.Log("[LSEnterMenu] HandleUIDeactivation");
        Panel.SetActive(false);
        panelIsActive = false;
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

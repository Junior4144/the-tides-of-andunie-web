using UnityEngine;

public class LSEnterMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;

    private bool Clicked = false;

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

    private void Start() => Panel.SetActive(true);

    private void HandleUIToggling()
    {
        Panel.SetActive(true);
    }

    public void HandleUIDeactivation()
    {
        Debug.Log("[LSEnterMenu] HandleUIDeactivation");
        Panel.SetActive(false);
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

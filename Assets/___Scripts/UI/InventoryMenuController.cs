using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    private void OnEnable()
    {
        UIEvents.OnRequestCloseAllUI += CloseInventory;
    }

    private void OnDisable()
    {
        UIEvents.OnRequestCloseAllUI -= CloseInventory;
    }

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        UIEvents.OnRequestInventoryToggle?.Invoke();
    //    }
    //}

    private void CloseInventory()
    {
        menuCanvas.SetActive(false);
    }
}

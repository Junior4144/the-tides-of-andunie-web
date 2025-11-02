using UnityEngine;

public class LSModelToggle : MonoBehaviour
{
    public MonoBehaviour PlayerMovement;

    private void OnEnable()
    {
        UIEvents.OnRequestShopToggle += HandleModelToggle;
        UIEvents.OnRequestInventoryToggle += HandleModelToggle;
    }
    private void OnDisable()    
    {   
        UIEvents.OnRequestShopToggle -= HandleModelToggle;
        UIEvents.OnRequestInventoryToggle -= HandleModelToggle;
    }

    private void HandleModelToggle()
    {
        bool newState = !PlayerMovement.enabled;

        PlayerMovement.enabled = newState;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class ShopHUDController : MonoBehaviour
{
    public void HandleHUDActivation()
    {
        UIEvents.OnRequestShopToggle?.Invoke();
    }
}


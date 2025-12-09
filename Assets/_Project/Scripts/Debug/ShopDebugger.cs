using UnityEngine;

public class ShopDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            UIEvents.OnRequestShopToggle?.Invoke();
        }
    }
}

using System;
using UnityEngine;

public class LSShopController : MonoBehaviour
{
    private bool isPlayerInside = false;
    public static event Action OnPlayerEnterSelectionZone;
    public static event Action OnPlayerExitSelectionZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!isPlayerInside)
        {
            isPlayerInside = true;
            OnPlayerEnterSelectionZone?.Invoke();

            Debug.Log("[LSShopController] Player entered shop zone");

            UIEvents.OnRequestShopToggle?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isPlayerInside)
        {
            isPlayerInside = false;
            OnPlayerExitSelectionZone?.Invoke();

            Debug.Log("[LSShopController] Player left shop zone");
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[LSShopController] Enter shop key pressed inside zone");
            UIEvents.OnRequestShopToggle?.Invoke();
        }
    }
}

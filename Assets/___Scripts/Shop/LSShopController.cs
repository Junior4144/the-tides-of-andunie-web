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

        UIEvents.OnRequestShopToggle?.Invoke();
        OnPlayerEnterSelectionZone?.Invoke();

        Debug.Log("[Level Selection] Player entered level zone");
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;


        OnPlayerExitSelectionZone?.Invoke();

        Debug.Log("[Level Selection] Player left level zone");
        isPlayerInside = false;
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

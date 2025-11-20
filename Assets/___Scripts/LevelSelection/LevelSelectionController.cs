using System;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] private string villageId;
    [SerializeField] bool isExit;
    [SerializeField] private string VillageLiberationID;

    private bool isPlayerInside = false;

    public static event Action OnPlayerEnterSelectionZone;
    public static event Action OnPlayerExitSelectionZone;

    public static event Action<string, string, bool> PlayerActivatedMenu;
    public string location = "DefaultSpawn";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnPlayerEnterSelectionZone?.Invoke();
        isPlayerInside = true;

        Debug.Log("[Level Selection] Player entered level zone");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnPlayerExitSelectionZone?.Invoke();
        isPlayerInside = false;
        
        Debug.Log("[Level Selection] Player left level zone");
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[Level Selection] Enter key pressed inside zone");
            OpenVillageEntryUI();
        }
    }

    private void OpenVillageEntryUI()
    {
        if (isExit)
            PlayerActivatedMenu?.Invoke("", location, isExit);
        else
            PlayerActivatedMenu?.Invoke(villageId, location, isExit);
    }

}

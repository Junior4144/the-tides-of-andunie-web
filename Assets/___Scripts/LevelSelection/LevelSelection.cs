using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private string villageId;
    [SerializeField] bool isExit;

    private bool isPlayerInside = false;

    public static event Action OnPlayerEnterSelectionZone;
    public static event Action OnPlayerExitSelectionZone;


    public static event Action EnterLeaveVillageZone;
    public static event Action ExitLeaveVillageZone;

    public static event Action<string, string, bool> PlayerActivatedMenu;

    public string location = "DefaultSpawn";
    public bool TriggerGlobalInvasion = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isExit) // Different Invoke because box are different sizes -> map are different scales
            EnterLeaveVillageZone?.Invoke();
        else OnPlayerEnterSelectionZone?.Invoke();

        Debug.Log("[Level Selection] Player entered level zone");
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isExit)
            ExitLeaveVillageZone?.Invoke();
        else OnPlayerExitSelectionZone?.Invoke();

        Debug.Log("[Level Selection] Player left level zone");
        isPlayerInside = false;
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[Level Selection] Enter key pressed inside zone");
            ProceedToNextStage();
        }
    }

    private void ProceedToNextStage()
    {
        if (isExit)
            PlayerActivatedMenu?.Invoke("EXIT", location, TriggerGlobalInvasion);
        else
            PlayerActivatedMenu?.Invoke(villageId, location, false);
    }

}

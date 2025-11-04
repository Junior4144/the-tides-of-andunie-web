using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private string villageId;
    [SerializeField] bool isExit;
    [SerializeField] private string VillageLiberationID;

    private bool isPlayerInside = false;

    public static event Action OnPlayerEnterSelectionZone;
    public static event Action OnPlayerExitSelectionZone;
    public static event Action EnterLeaveVillageZone;
    public static event Action ExitLeaveVillageZone;
    public static event Action<string, string> PlayerActivatedMenu;

    public string location = "DefaultSpawn";
    public bool TriggerGlobalInvasion = false;
    public bool LiberateVillage = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isExit)
            EnterLeaveVillageZone?.Invoke();
        else 
            OnPlayerEnterSelectionZone?.Invoke();

        Debug.Log("[Level Selection] Player entered level zone");
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isExit)
            ExitLeaveVillageZone?.Invoke();
        else 
            OnPlayerExitSelectionZone?.Invoke();

        isPlayerInside = false;
        Debug.Log("[Level Selection] Player left level zone");
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
            PlayerActivatedMenu?.Invoke("EXIT", location);
        else
            PlayerActivatedMenu?.Invoke(villageId, location);
    }

}

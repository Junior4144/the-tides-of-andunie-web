using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private string villageId;

    private bool isPlayerInside = false;

    public static LevelSelection instance;

    public static event Action OnPlayerEnterSelectionZone;
    public static event Action OnPlayerExitSelectionZone;
    public static event Action<string, string> PlayerActivatedMenu;

    public string location = "DefaultSpawn";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

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
            
            Debug.Log("[Level Selection] Enter key pressed inside zone");
            ProceedToNextStage();
        }
    }

    private void ProceedToNextStage()
    {
        PlayerActivatedMenu?.Invoke(villageId, location); // change to ID
        PlayerManager.Instance.gameObject.GetComponent<PlayerHeroMovement>().enabled = false;
    }

}

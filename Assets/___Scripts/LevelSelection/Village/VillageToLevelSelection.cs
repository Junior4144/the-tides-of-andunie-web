using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageToLevelSelection : MonoBehaviour
{
    private string nextScene = "LevelSelector";

    private bool isPlayerInside = false;

    public static VillageToLevelSelection instance;

    public static event Action OnPlayerEnterVillageLeave;
    public static event Action OnPlayerExitVillageLeave;
    public static event Action<string, string> PlayerActivatedLeaveVillageMenu;

    public static event Action<string> StartInvasion;

    public string location = "DefaultSpawn";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnPlayerEnterVillageLeave?.Invoke();
        Debug.Log("[VillageToLevelSelection] Player entered Village Leave zone");
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnPlayerExitVillageLeave?.Invoke();
        Debug.Log("[VillageToLevelSelection] Player left Village leave zone");
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
        PlayerActivatedLeaveVillageMenu?.Invoke(nextScene, location);
        if(location == "SpawnVillage7")
        {

        }
        PlayerManager.Instance.gameObject.GetComponent<PlayerHeroMovement>().enabled = false;
    }

    public void NextStage() => LoadNextStage();

    private void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}

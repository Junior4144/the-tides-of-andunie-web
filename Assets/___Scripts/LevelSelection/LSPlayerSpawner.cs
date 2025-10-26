using System.Linq;
using UnityEngine;

public class LSPlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;

    [SerializeField] private GameObject[] spawnPoints;

    private void Awake() => OnSceneLoaded();

    private void OnSceneLoaded()
    {
        if (PlayerManager.Instance != null) return;
        GameObject spawnPoint;
        string last;

        if (SaveManager.Instance)
        {
            last = SaveManager.Instance.CurrentSave.lastLocation;
            Debug.Log($"[LevelSelectionSpawner] last Location = {last}");
            spawnPoint = spawnPoints.FirstOrDefault(sp => sp.name == last);
        }
        else {
            last = "DefaultSpawn";
            Debug.Log($"[LevelSelectionSpawner] last Location = {last}");
            spawnPoint = spawnPoints.FirstOrDefault(sp => sp.name == last);
        }

        currentPlayer = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        currentPlayer.transform.rotation = spawnPoint.transform.rotation;
        Debug.Log("New Player created");

        if (SaveManager.Instance && HealthUIController.Instance.Check_HealthBar_UI_IsActive())
        {
            SaveManager.Instance.RestorePlayerStats();

            var healthController = PlayerManager.Instance.GetComponentInChildren<IHealthController>();
            HealthUIController.Instance.UpdateHealthBar(healthController.GetCurrentHealth(), healthController.GetMaxHealth());
        }
    }
}

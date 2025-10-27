using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;

    private void Awake() =>
        OnSceneLoaded();

    private void OnSceneLoaded()
    {
        if (PlayerManager.Instance != null) return;

        //Default Player Spawner Transform
        currentPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        string sceneName = gameObject.scene.name;
        Debug.Log($"PlayerSpawner: Scene: {sceneName} trying to send to dictionary");

        var savedTransform = SceneSavePositionManager.Instance?.GetSavedPosition(sceneName);

        Vector3 spawnPos;
        Quaternion spawnRot;

        if (savedTransform != null)
        {
            spawnPos = savedTransform.Value.pos;
            spawnRot = savedTransform.Value.rot;
            Debug.Log($"Spawning player at SAVED position for scene: {sceneName}");
        }
        else // equals null
        {
            spawnPos = transform.position;
            spawnRot = transform.rotation;
            Debug.Log($"Spawning player at DEFAULT spawner position in scene: {sceneName}");
        }

        PlayerManager.Instance.SetPlayerTransform(spawnPos, spawnRot);

        Debug.Log("New Player created");

        if (SaveManager.Instance && HealthUIController.Instance.Check_HealthBar_UI_IsActive())
        {
            SaveManager.Instance.RestorePlayerStats();

            var healthController = PlayerManager.Instance.GetComponentInChildren<IHealthController>();
            HealthUIController.Instance.UpdateHealthBar(healthController.GetCurrentHealth(), PlayerStatsManager.Instance.MaxHealth); 
        }
        
    }

}

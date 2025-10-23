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

        currentPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        Debug.Log("New Player created");

        if (SaveManager.Instance && HealthUIController.Instance.Check_HealthBar_UI_IsActive())
        {
            SaveManager.Instance.RestorePlayerStats();

            var healthController = PlayerManager.Instance.GetComponentInChildren<IHealthController>();
            HealthUIController.Instance.UpdateHealthBar(healthController.GetCurrentHealth(), healthController.GetMaxHealth());
        }
        
    }

    private void Update()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log($"[PlayerSpawner] {activeScene}");

        if (gameObject.scene == activeScene)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log($"PlayerSpawner: ACtive Scene: {sceneName} trying to send to dictionary");

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

            enabled = false;
        }
    }
}

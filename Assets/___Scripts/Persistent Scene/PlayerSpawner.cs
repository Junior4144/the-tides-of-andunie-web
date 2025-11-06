using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Awake() => SpawnPlayer();

    private void SpawnPlayer()
    {
        Debug.Log("[PlayerSpawner] Spawning player");
        Instantiate(playerPrefab, transform.position, Quaternion.identity);

        var (spawnPos, spawnRot) = DetermineSpawnTransform();
        PlayerManager.Instance.SetPlayerTransform(spawnPos, spawnRot);

        Debug.Log("[PlayerSpawner] Player spawned successfully");

        RestorePlayerStats();
    }

    private (Vector3 pos, Quaternion rot) DetermineSpawnTransform()
    {
        var savedTransform = SceneSavePositionManager.Instance?.GetSavedPosition(gameObject.scene.name);

        if (savedTransform.HasValue)
        {
            Debug.Log($"[PlayerSpawner] Using saved position {savedTransform.Value.pos}");
            return savedTransform.Value;
        }

        transform.GetPositionAndRotation(out var defaultPos, out var defaultRot);
        Debug.Log($"[PlayerSpawner] Using default position {defaultPos}");
        return (defaultPos, defaultRot);
    }

    private void RestorePlayerStats()
    {
        if (!SaveManager.Instance || !HealthUIController.Instance.Check_HealthBar_UI_IsActive())
            return;

        SaveManager.Instance.RestorePlayerStats();

        HealthUIController.Instance.UpdateHealthBar(
            currentHealth: PlayerManager.Instance.GetCurrentHealth(),
            maxhealth: PlayerStatsManager.Instance.MaxHealth
        );
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerSquadSpawner : MonoBehaviour
{
    [SerializeField] private KeyCode spawnKey = KeyCode.M;

    void Update()
    {
        if (Input.GetKeyDown(spawnKey)) SpawnSquadFormation();
    }

    private void SpawnSquadFormation()
    {
        if (SquadFormationManager.Instance == null)
        {
            Debug.LogError("[PlayerSquadSpawner] SquadFormationManager.Instance is null");
            return;
        }

        Vector2 playerPos = transform.position;
        IReadOnlyList<FormationPosition> formation = SquadFormationManager.Instance.GetFormation();

        LogSpawnStart(playerPos, formation.Count);

        foreach (var unit in formation)
            SpawnUnit(unit, playerPos);
    }

    private void SpawnUnit(FormationPosition unit, Vector2 playerPos)
    {
        GameObject prefab = SquadFormationManager.Instance.GetPrefab(unit.unitType);

        if (!ValidatePrefab(prefab, unit.unitType)) return;

        Vector2 spawnPos = playerPos + unit.offset;
        GameObject spawned = Instantiate(prefab, spawnPos, Quaternion.identity);

        InitializeFollower(spawned, unit.offset);
        LogUnitSpawned(unit.unitType, spawnPos, unit.offset);
    }

    private bool ValidatePrefab(GameObject prefab, UnitType unitType)
    {
        if (prefab != null) return true;

        Debug.LogError($"[PlayerSquadSpawner] No prefab found for {unitType}");
        return false;
    }

    private void InitializeFollower(GameObject spawned, Vector2 offset)
    {
        if (spawned.TryGetComponent<PlayerSquadFollower>(out var follower))
        {
            follower.Initialize(offset);
        }
    }

    private void LogSpawnStart(Vector2 playerPos, int unitCount)
    {
        Debug.Log($"[PlayerSquadSpawner] ===Spawning Squad===");
        Debug.Log($"[PlayerSquadSpawner] Player position {playerPos}");
        Debug.Log($"[PlayerSquadSpawner] Total units {unitCount}");
    }

    private void LogUnitSpawned(UnitType unitType, Vector2 spawnPos, Vector2 offset)
    {
        Debug.Log($"[PlayerSquadSpawner] Spawned unit {unitType}");
        Debug.Log($"[PlayerSquadSpawner] Spawn position {spawnPos}");
        Debug.Log($"[PlayerSquadSpawner] Offset {offset}");
    }
}

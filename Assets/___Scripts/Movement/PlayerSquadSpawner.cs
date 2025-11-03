using System.Collections.Generic;
using System.Linq;
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
        if (!TryGetManager(out var manager)) return;

        Vector2 playerPos = transform.position;
        var formation = manager.GetFormation();

        LogSpawnStart(playerPos, formation.Count);
        SpawnAllUnits(formation, playerPos);
    }

    private void SpawnAllUnits(IReadOnlyList<FormationPosition> formation, Vector2 playerPos)
    {
        foreach (var unit in formation)
            SpawnUnitAtOffset(unit, playerPos);
    }

    private void SpawnUnitAtOffset(FormationPosition unit, Vector2 playerPos)
    {
        GameObject prefab = SquadFormationManager.Instance.GetPrefab(unit.unitType);

        if (prefab == null)
        {
            Debug.LogError($"[PlayerSquadSpawner] No prefab found for {unit.unitType}");
            return;
        }

        Vector2 spawnPos = playerPos + unit.offset;
        GameObject spawned = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (spawned.TryGetComponent<PlayerSquadFollower>(out var follower))
        {
            follower.Initialize(unit.offset);
        }

        LogUnitSpawned(unit.unitType.ToString(), spawnPos, unit.offset);
    }

    private bool TryGetManager(out SquadFormationManager manager)
    {
        manager = SquadFormationManager.Instance;

        if (manager != null) return true;

        Debug.LogError("[PlayerSquadSpawner] FormationSquadManager.Instance is null. Cannot spawn squad.");
        return false;
    }

    private void LogSpawnStart(Vector2 playerPos, int unitCount)
    {
        Debug.Log($"[PlayerSquadSpawner] ===Spawning Squad===");
        Debug.Log($"[PlayerSquadSpawner] Player position {playerPos}");
        Debug.Log($"[PlayerSquadSpawner] Total units {unitCount}");
    }

    private void LogUnitSpawned(string unitName, Vector2 spawnPos, Vector2 offset)
    {
        Debug.Log($"[PlayerSquadSpawner] Spawned unit {unitName}");
        Debug.Log($"[PlayerSquadSpawner] Spawn position {spawnPos}");
        Debug.Log($"[PlayerSquadSpawner] Offset {offset}");
    }
}

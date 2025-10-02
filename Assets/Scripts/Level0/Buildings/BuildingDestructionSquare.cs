using UnityEngine;

public class BuildingDestructionSquare : BuildingDestructable
{
    [SerializeField]
    private Vector2[] fireOffsets =
    {
        Vector2.zero,
        new (1f, 0f),
        new (-1f, 0f),
        new (0f, 1f),
        new(0f, -1f)
    };
    protected override void HandleExplosion()
    {
        if (hasExploded) return;

        Vector2 screenPosition = GetScreenPosition();

        if (!(CheckCameraBoundaries(screenPosition))) return;

        SpawnExplosion();
        SpawnFire();
        hasExploded = true;
    }
    void SpawnFire()
    {
        foreach (var offset in fireOffsets)
            Instantiate(fire, transform.position + (Vector3)offset, Quaternion.identity);
    }


}

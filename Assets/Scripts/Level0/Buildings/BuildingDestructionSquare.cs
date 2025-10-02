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

    [SerializeField]
    private AudioClip _explosionSound;

    protected override void HandleExplosion()
    {
        SpawnExplosion();
        SpawnFire(fireOffsets);
        SpawnFireSound();
        hasExploded = true;
        PlayExplosionSound(_explosionSound);
    }
}

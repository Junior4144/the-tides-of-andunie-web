using UnityEngine;

public class BuildingDestructionRectangle : BuildingDestructable
{
    [SerializeField]
    private Vector2[] fireOffsets =
    {
        Vector2.zero,
        new (1f, 0f),
        new (-1f, 0f),
        new (0f, 2f),
        new (0f, -2f)
    };

    [SerializeField]
    private AudioClip _explosionSound;

    protected override void HandleExplosion()
    {
        SpawnExplosion();
        ReplaceSprite();

        SpawnNewFire();
        SpawnFireSound();
        hasExploded = true;
        PlayExplosionSound(_explosionSound);
    }
}

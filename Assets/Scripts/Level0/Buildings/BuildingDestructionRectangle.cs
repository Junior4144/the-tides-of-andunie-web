using UnityEngine;

public class BuildingDestructionRectangle : BuildingDestructable
{
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

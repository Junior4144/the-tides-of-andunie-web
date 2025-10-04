using UnityEngine;

public class BuildingDestructionSquare : BuildingDestructable
{
    [SerializeField]
    private AudioClip _explosionSound;

    protected override void HandleExplosion()
    {
        SpawnExplosion();
        PlayExplosionSound(_explosionSound);

        ReplaceSprite();

        SpawnNewFire();
        SpawnFireSound();

        if(!hasExploded) hasExploded = true;
    }
}

using UnityEngine;

public class BuildingDestructionCircle : BuildingDestructable
{
    [SerializeField]
    private Vector2[] fireOffsets =
    {
        Vector2.zero,
        new (1f, 0f),
        new (-1f, 0f),
        new (0f, 1f),
        new (0f, -1f)
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
    //public void SpawnCircleFire()
    //{
    //    Instantiate(fireSprite, _fire_1.transform.position, Quaternion.identity);
    //    Instantiate(fireSprite, _fire_2.transform.position, Quaternion.identity);
    //    Instantiate(fireSprite, _fire_3.transform.position, Quaternion.identity);
    //}

}

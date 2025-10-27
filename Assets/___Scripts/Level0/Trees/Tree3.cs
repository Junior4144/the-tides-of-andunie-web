using UnityEngine;

public class Tree3 : TreeFire
{
    protected override void SpawnNewFire()
    {
        Instantiate(fireSprite_1, _fire_position_1.transform.position, Quaternion.identity);
        Instantiate(fireSprite_2, _fire_position_2.transform.position, Quaternion.identity);
        Instantiate(fireSprite_3, _fire_position_3.transform.position, Quaternion.identity);
        Instantiate(fireSprite_4, _fire_position_4.transform.position, Quaternion.identity);

    }
    protected void SpawnFireSound()
    {
        if (fireSound != null)
            Instantiate(fireSound, transform.position, Quaternion.identity, transform);
    }
}

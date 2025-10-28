using UnityEngine;
using UnityEngine.SceneManagement;

public class Tree5 : TreeFire
{

    protected override void SpawnNewFire()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "LevelSelector")
        {
            Instantiate(fireSprite_1, _fire_position_list[0].transform.position, Quaternion.identity);
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            Instantiate(fireSprite_1, _fire_position_list[i].transform.position, Quaternion.identity);
        }
    }
    protected void SpawnFireSound()
    {
        if (fireSound != null)
            Instantiate(fireSound, transform.position, Quaternion.identity, transform);
    }
}

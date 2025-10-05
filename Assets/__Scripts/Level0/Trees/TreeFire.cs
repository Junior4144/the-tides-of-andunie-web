using UnityEngine;

public abstract class TreeFire : MonoBehaviour
{
    public GameObject fireSound;

    [SerializeField]
    protected GameObject _fire_position_1;
    [SerializeField]
    protected GameObject _fire_position_2;
    [SerializeField]
    protected GameObject _fire_position_3;
    [SerializeField]
    protected GameObject _fire_position_4;
    [SerializeField]
    protected GameObject _fire_position_5;
    [SerializeField]
    protected GameObject _fire_position_6;

    [SerializeField]
    protected GameObject fireSprite_1;
    [SerializeField]
    protected GameObject fireSprite_2;
    [SerializeField]
    protected GameObject fireSprite_3;
    [SerializeField]
    protected GameObject fireSprite_4;
    [SerializeField]
    protected GameObject fireSprite_5;
    [SerializeField]
    protected GameObject fireSprite_6;

    private void Start()
    {
        SpawnNewFire();
    }

    protected abstract void SpawnNewFire();
}

using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField]
    private float _damage = 20;

    [SerializeField]
    private string _layerName;

    void Start()
    {
        Debug.Log("Started Melee Script");
    }


    void Update()
    {}

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (IsEnemy(otherCollider) && otherCollider.GetComponent<HealthController>())
        {
            Attack(otherCollider.gameObject);
        }
    }

    private bool IsEnemy(Collider2D otherCollider) => otherCollider.gameObject.layer == LayerMask.NameToLayer(_layerName);

    private void Attack(GameObject enemyObject)
    {
        enemyObject.GetComponent<HealthController>().TakeDamage(_damage);
    }
}

using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField]
    private float _damage = 20;

    void Start()
    {
        Debug.Log("Started Melee Script");
    }


    void Update()
    {}

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log("Collision Detected");

        if (IsEnemy(otherCollider))
        {
            Attack(otherCollider.gameObject);
        }
    }

    private bool IsEnemy(Collider2D otherCollider) => otherCollider.gameObject.layer == LayerMask.NameToLayer("Enemy");

    private void Attack(GameObject enemyObject)
    {
        Debug.Log("Enemy Hit");
        enemyObject.GetComponent<HealthController>().TakeDamage(_damage);
    }
}

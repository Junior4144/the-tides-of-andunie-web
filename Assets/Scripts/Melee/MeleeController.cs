using UnityEngine;

public class MeleeController : MonoBehaviour
{
    // Allows you to change private variables in Unity
    [SerializeField]
    private float _damage = 20;

    [SerializeField]
    private LayerMask _layermaskOfEnemy;

    [SerializeField]
    private GameObject _attackPoint;

    [SerializeField]
    private float _attackCircleRadius = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called from animation keyframe
    public void attack()
    {
        // An array of all the enemies in a circular radius around the weapon
        Collider2D[] enemy = Physics2D.OverlapCircleAll(_attackPoint.transform.position, _attackCircleRadius, _layermaskOfEnemy);

        foreach (Collider2D enemyGameObject in enemy)
        {
            Debug.Log("Hit enemy");
            enemyGameObject.GetComponent<HealthController>().TakeDamage(_damage);
        }
    }
    
    // Draws the attack circle
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.transform.position, _attackCircleRadius);
    }
}

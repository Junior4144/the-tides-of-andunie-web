using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerExplosion : MonoBehaviour
{
    [SerializeField]
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}

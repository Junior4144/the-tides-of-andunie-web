using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Unity.Cinemachine;
public class PlayerExplosion : MonoBehaviour
{
    [SerializeField]
    public GameObject explosion;
    [SerializeField]
    public float duration;
    [SerializeField]
    public float magnitude;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}

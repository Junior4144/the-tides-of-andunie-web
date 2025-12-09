using UnityEngine;

public class VillagerDestroy : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bounds")) return;

        if (collision.gameObject.CompareTag("ForestTrees"))
            Destroy(gameObject);
    }
}

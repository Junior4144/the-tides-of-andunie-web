using UnityEngine;

public class VillagerDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bounds")) return;

        if (collision.gameObject.CompareTag("ForestTrees"))
            Destroy(gameObject);

    }
}

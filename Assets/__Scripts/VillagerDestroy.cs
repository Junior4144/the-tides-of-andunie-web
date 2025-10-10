using UnityEngine;

public class VillagerDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bounds")) return;

        if (collision.gameObject.CompareTag("ForestTrees"))
            Destroy(gameObject);

    }
}

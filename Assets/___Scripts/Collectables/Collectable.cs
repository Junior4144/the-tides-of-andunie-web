using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableData data;
    public CollectableData Data => data;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CollectableManager.Instance.HandleCollect(data, this);

    }
}
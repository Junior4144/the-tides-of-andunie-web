using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void HandleCollect(CollectableData data, Collectable collectableGameObject)
    {
        //prevents pickup if max health
        if (data.type == CollectableType.Health && PlayerManager.Instance.GetPercentHealth() > 0.98f) { Debug.Log("Health full — do not pick up."); return; }

        switch (data.type)
        {
            case CollectableType.Coin:
                InventoryManager.Instance.AddCoins(data);
                break;

            case CollectableType.Health:
                PlayerManager.Instance.AddHealth(data.amount);
                break;
        }

        if (data.destroyOnPickup)
            Object.Destroy(collectableGameObject.gameObject);

        //fire a global event here for UI updates etc
    }
}

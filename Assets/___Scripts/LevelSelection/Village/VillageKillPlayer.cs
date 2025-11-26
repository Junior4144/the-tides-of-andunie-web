using UnityEngine;

public class VillageKillPlayer : MonoBehaviour
{
    
    public void HandleResettingVillageLevel()
    {
        PlayerManager.Instance.gameObject.GetComponentInChildren<HealthController>().TakeDamage(999);
    }
}

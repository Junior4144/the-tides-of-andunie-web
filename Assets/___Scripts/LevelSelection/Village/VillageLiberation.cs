using UnityEngine;

public class VillageLiberation : MonoBehaviour
{
    [SerializeField]
    private string villageId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //state this village is liberated
        LSManager.Instance.SetVillageState(villageId, VillageState.Liberated_Done);
        Debug.Log($"[VillageLiberation] {villageId} has bee liberated");
    }
}

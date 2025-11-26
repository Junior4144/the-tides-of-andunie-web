using UnityEngine;

public class VillageLiberation : MonoBehaviour
{
    [SerializeField]
    private string villageId;

    private bool isTrigger = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger) return;

        //state this village is liberated
        LSManager.Instance.SetVillageState(villageId, VillageState.Liberated_Done);
        Debug.Log($"[VillageLiberation] {villageId} has bee liberated");

        isTrigger = true;
    }
}

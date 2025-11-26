using UnityEngine;

public class LSGlobalInvasion : MonoBehaviour
{
    private bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger) return;

        LSManager.Instance.TriggerGlobalInvasion();
        Debug.Log("Global invasion starting from Level 1");
        isTrigger = true;
    }
}

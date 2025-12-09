using UnityEngine;

public class VillagePointerTargetController : MonoBehaviour
{
    public GameObject navigationTarget;

    private void Awake()
    {
        if (transform.childCount > 0)
            navigationTarget = transform.GetChild(0).gameObject;
        else
            Debug.LogWarning($"{name} has no child object navigationTarget was not set.");
    }

}

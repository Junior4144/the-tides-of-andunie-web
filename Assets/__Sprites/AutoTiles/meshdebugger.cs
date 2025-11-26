using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ForceMeshSorting : MonoBehaviour
{
    public string sortingLayerName = "BakedOuter";
    public int sortingOrder = 0;

    void Start()
    {
        var r = GetComponent<MeshRenderer>();
        r.sortingLayerName = sortingLayerName;
        r.sortingOrder = sortingOrder;
    }
}
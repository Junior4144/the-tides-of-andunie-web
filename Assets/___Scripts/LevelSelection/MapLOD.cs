using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLOD : MonoBehaviour
{
    private Camera cam;
    public float t1 = 100f;
    public float t2 = 200f;
    public float t3 = 250f;

    public GameObject Quad4Oceans;


    public GameObject tilemap;
    public MeshRenderer quadRenderer;
    public Texture2D lod1;
    public Texture2D lod2;

    public string oceanSortingLayer = "Default";
    public int oceanSortingOrder = 0;

    public string quadSortingLayer = "Default";
    public int quadSortingOrder = 1;


    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();

        quadRenderer.sortingLayerName = quadSortingLayer;
        quadRenderer.sortingOrder = quadSortingOrder;

        var oceanRenderer = Quad4Oceans.GetComponent<Renderer>();
        if (oceanRenderer != null)
        {
            oceanRenderer.sortingLayerName = oceanSortingLayer;
            oceanRenderer.sortingOrder = oceanSortingOrder;
        }
    }

    void Update()
    {
        float zoom = cam.orthographicSize;

        if (zoom <= t1)
        {
            Quad4Oceans.SetActive(true);

            tilemap.SetActive(true);
        }
        else if (zoom <= t2)
        {
            tilemap.SetActive(false);
            
            Quad4Oceans.SetActive(true);
            quadRenderer.gameObject.SetActive(true);
            quadRenderer.material.mainTexture = lod1;
        }
        else if (zoom <= t3)
        {
            Quad4Oceans.SetActive(true);
            
            tilemap.SetActive(false);
            quadRenderer.gameObject.SetActive(true);
            quadRenderer.material.mainTexture = lod2;
        }
    }
}

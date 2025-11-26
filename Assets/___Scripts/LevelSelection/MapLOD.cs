using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLOD : MonoBehaviour
{
    private Camera cam;                 // assign Main Camera
    public float t1 = 100f;            // switch at > 100
    public float t2 = 200f;            // switch at > 200
    public float t3 = 250f;            // switch at > 300

    public GameObject Quad4Oceans;


    public GameObject tilemap;
    public MeshRenderer quadRenderer;
    public Texture2D lod1;             // 100-200 8k
    public Texture2D lod2;             // 200-300 4k

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
            // use tilemap, hide RT quad
            Quad4Oceans.SetActive(true);

            tilemap.SetActive(true);
            //quadRenderer.gameObject.SetActive(false);
        }
        else if (zoom <= t2)
        {
            //Quad4Oceans.SetActive(false);
            tilemap.SetActive(false);
            // first RT
            Quad4Oceans.SetActive(true);
            quadRenderer.gameObject.SetActive(true);
            quadRenderer.material.mainTexture = lod1;
        }
        else if (zoom <= t3)
        {
            Quad4Oceans.SetActive(true);
            //Quad4Oceans.SetActive(false);
            tilemap.SetActive(false);
            quadRenderer.gameObject.SetActive(true);
            quadRenderer.material.mainTexture = lod2;
        }
        //else
        //{
        //    Quad4Oceans.SetActive(false);

        //    quadRenderer.gameObject.SetActive(true);
        //    quadRenderer.material.mainTexture = lod3;
        //}
    }
}

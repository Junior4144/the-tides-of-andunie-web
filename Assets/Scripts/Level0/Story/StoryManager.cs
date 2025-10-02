using System.Collections;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public string spawnerTag = "CannonSpawner";
    Camera _camera;
    [SerializeField]
    public GameObject spawnerParent;
    [SerializeField] private CameraMovement cameraMovement;

    [SerializeField] private GameObject playerHero;

    private void Awake()
    {
        _camera = Camera.main; 
    }
    void Start() =>
        StartCoroutine(RunEvents());


    IEnumerator RunEvents()
    {
        Debug.Log("Scene Started");

        yield return new WaitForSeconds(3f);

        //move camera towards ocean
        //return towards middle
        CameraSlide CameraSldier = _camera.GetComponent<CameraSlide>();
        CameraSldier.SlideCamera();
        PlayerHeroMovement movement = playerHero.GetComponent<PlayerHeroMovement>();
        movement.enabled = false;

        //activateSpawners

        //all tribal people run away

        yield return new WaitForSeconds(3f);
        movement.enabled = true;
        ActivateAllSpawners();

        //Camera start slideScrolling
        cameraMovement.enabled = true;

    }
    void ActivateAllSpawners() =>
        ActivateAllChildren(spawnerParent);

    void ActivateAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(true);
            ActivateAllChildren(child.gameObject); 
        }
    }
}

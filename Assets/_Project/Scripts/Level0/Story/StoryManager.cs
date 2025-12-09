using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public string spawnerTag = "CannonSpawner";

    [SerializeField] private GameObject spawnerParent;
    [SerializeField] private GameObject playerHero;
    [SerializeField] private CameraTarget target;

    public CinemachineCamera playerCam;
    public CinemachineCamera cutsceneCam;
    public float cutsceneDuration = 3f;

    void Start() =>
        StartCoroutine(RunEvents());
        
    IEnumerator RunEvents()
    {
        ActivateAllSpawners();

        StartSlideScroll();
        yield return null;
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
    void StartSlideScroll() =>
        target.enabled = true;




}

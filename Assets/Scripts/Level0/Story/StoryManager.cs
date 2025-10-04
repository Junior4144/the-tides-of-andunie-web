using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public string spawnerTag = "CannonSpawner";

    [SerializeField] private GameObject spawnerParent;
    [SerializeField] private GameObject playerHero;
    [SerializeField] private CameraTarget target;

    [SerializeField]
    public CinemachineCamera playerCam;
    [SerializeField]
    public CinemachineCamera cutsceneCam;
    [SerializeField]
    public float cutsceneDuration = 3f;

    void Start() =>
        StartCoroutine(RunEvents());
        
    IEnumerator RunEvents()
    {
        //PlayerHeroMovement movement = playerHero.GetComponent<PlayerHeroMovement>();

        //DisablePlayerMovement(movement);

        //yield return new WaitForSeconds(3f);

        //MovingCameraTowardsShip();
        //yield return new WaitForSeconds(3f);
        //EnablePlayerMovement(movement);

        //ActivateAllSpawners();

        StartSlideScroll();
        yield return null;
    }

    void DisablePlayerMovement(PlayerHeroMovement movement) => movement.enabled = false;
    void EnablePlayerMovement(PlayerHeroMovement movement) => movement.enabled = true;

    void MovingCameraTowardsShip()
    {
        cutsceneCam.Priority = 20;
        StartCoroutine(ReturnToPlayer());
    }
    IEnumerator ReturnToPlayer()
    {
        yield return new WaitForSeconds(cutsceneDuration);

        cutsceneCam.Priority = 0;
        playerCam.Priority = 20;
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

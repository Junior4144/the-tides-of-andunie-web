using System.Collections;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public string spawnerTag = "CannonSpawner";

    [SerializeField] private GameObject spawnerParent;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameObject playerHero;

    private Camera _camera;

    private void Awake() =>
        _camera = Camera.main; 
    void Start() =>
        StartCoroutine(RunEvents());

    IEnumerator RunEvents()
    {
        PlayerHeroMovement movement = playerHero.GetComponent<PlayerHeroMovement>();

        DisablePlayerMovement(movement);

        yield return new WaitForSeconds(3f);

        MovingCameraTowardsShip();

        yield return new WaitForSeconds(3f);

        EnablePlayerMovement(movement);

        ActivateAllSpawners();

        ActivateCameraSlideScroll();
    }

    void DisablePlayerMovement(PlayerHeroMovement movement) => movement.enabled = false;
    void EnablePlayerMovement(PlayerHeroMovement movement) => movement.enabled = true;

    void MovingCameraTowardsShip()
    {
        CameraSlide CameraSldier = _camera.GetComponent<CameraSlide>();
        CameraSldier.SlideCamera();
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

    void ActivateCameraSlideScroll() =>
        cameraMovement.enabled = true;

}

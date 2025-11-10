using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private float occurAfterVelocity = 5f;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float footStepSpread = 0.2f;
    
    [SerializeField] private Rigidbody2D playerRigidbody;

    private ParticleSystem footStepParticles;
    private float counter = 0f;
    private bool isLeftFoot = true;

    private LSBuildingsInvasion Building;

    GameObject[] smokeChildren = new GameObject[2];


    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void Start()
    {
        footStepParticles = GetComponentInChildren<ParticleSystem>();
        Building = GetComponentInParent<LSBuildingsInvasion>();
    }

    void Update()
    {
        if (playerRigidbody == null) return;

        if (playerRigidbody.linearVelocity.magnitude >= occurAfterVelocity)
        {
            counter += Time.deltaTime;

            if (counter >= spawnInterval)
            {
                SpawnFootstep();
                counter = 0f;
                isLeftFoot = !isLeftFoot;
            }
        }
        else
            counter = 0f;
    }

    void SpawnFootstep()
    {
        float side = isLeftFoot ? -footStepSpread : footStepSpread;
        transform.localPosition = new Vector3(side, 0, 0);
        footStepParticles.Play();
    }

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            HandleParticleSpawning();
    }

    void HandleParticleSpawning()
    {
        int count = transform.childCount;
       ;

        for (int i = 0; i < count; i++)
        {
            smokeChildren[i] = transform.GetChild(i).gameObject;
        }


        Building = GetComponentInParent<LSBuildingsInvasion>();
        SpawnSmoke();
    }

    void SpawnSmoke()
    {
        if (Building == null)
        {
            smokeChildren[0].SetActive(true);
            smokeChildren[0].GetComponent<ParticleSystem>().Play();
            return;
        }

        if (LSManager.Instance == null)
        {
            Debug.LogWarning("LSManager.Instance not found!");
            return;
        }

        if (Building.VillageId != null && LSManager.Instance.GetVillageState(Building.VillageId) == VillageState.Invaded)
        {
            smokeChildren[0].SetActive(false);
            smokeChildren[1].SetActive(true);
            smokeChildren[1].GetComponent<ParticleSystem>().Play();
        }
           
    }




}
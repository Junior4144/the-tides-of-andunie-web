using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireworksController : MonoBehaviour
{
    [Header("Firework Settings")]
    public FireworkSet fireworkSet;

    private string villageId = "";

    private CircleCollider2D spawnArea;
    private AudioSource _audioSource;

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void Awake()
    {
        spawnArea = GetComponentInParent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            HandleSetup();
    }

    private void HandleSetup()
    {
        if (GlobalStoryManager.Instance.HasExitedLiberation)
        {
            villageId = GlobalStoryManager.Instance.LastLiberatedVillageID;

            if (string.IsNullOrEmpty(villageId))
                return;

            spawnArea = FindSpawnAreaByVillageName(villageId);

            if (spawnArea == null)
            {
                Debug.LogWarning("Could not find village: " + villageId);
                return;
            }

            StartCoroutine(PlayWaves());
        }

    }

    private IEnumerator PlayWaves()
    {
        _audioSource.PlayOneShot(fireworkSet.fireworkClip);

        for (int wave = 0; wave < fireworkSet.spawnCounts.Length; wave++)
        {
            int count = fireworkSet.spawnCounts[wave];
            float delay = fireworkSet.waveDelays[wave];

            for (int i = 0; i < count; i++)
            {
                Vector2 pos = RandomPointInCircle(spawnArea);

                GameObject chosen = fireworkSet.fireworkPrefabs[
                    Random.Range(0, fireworkSet.fireworkPrefabs.Length)
                ];

                Instantiate(chosen, pos, Quaternion.identity);

                // 💥 Small random delay BETWEEN each firework
                yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
            }

            // Wait between waves
            yield return new WaitForSeconds(delay);
        }
    }

    CircleCollider2D FindSpawnAreaByVillageName(string nameToFind)
    {
        foreach (Transform child in transform)
        {
            if (child.name == nameToFind)
            {
                return child.GetComponent<CircleCollider2D>();
            }
        }

        return null;
    }

    Vector2 RandomPointInCircle(CircleCollider2D circle)
    {
        Vector2 center = circle.transform.position;
        float r = circle.radius * circle.transform.lossyScale.x;

        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Mathf.Sqrt(Random.Range(0f, 1f)) * r;

        return center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }
}
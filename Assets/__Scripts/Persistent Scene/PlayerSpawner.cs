using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;

    [SerializeField] private HealthBarUI _healthBarUI;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PersistentScene") return; // skip

        Vector3 spawnPosition = Vector3.zero;

        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("New Player created");
        //update healthbar
        SaveManager.Instance.RestorePlayerStats();

        var healthController = PlayerManager.Instance.GetComponentInChildren<IHealthController>();

        _healthBarUI.UpdateHealthBar(healthController.GetCurrentHealth(), healthController.GetMaxHealth());
    }
}

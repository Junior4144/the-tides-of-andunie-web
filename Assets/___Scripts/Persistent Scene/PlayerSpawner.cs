using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;

    private void Awake() =>
        OnSceneLoaded();

    private void OnSceneLoaded()
    {
        if (PlayerManager.Instance != null) return;

        currentPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        currentPlayer.transform.rotation = transform.rotation;

        Debug.Log("New Player created");

        if (SaveManager.Instance && UIManager.Instance.Check_HealthBar_UI_IsActive())
        {
            SaveManager.Instance.RestorePlayerStats();

            var healthController = PlayerManager.Instance.GetComponentInChildren<IHealthController>();
            UIManager.Instance.UpdateHealthBar(healthController.GetCurrentHealth(), healthController.GetMaxHealth());
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;

    //[SerializeField] private HealthBarUI _healthBarUI;

    private void Awake()
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        currentPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        currentPlayer.transform.rotation = transform.rotation;
        Debug.Log("New Player created");

        if (SaveManager.Instance && UIManager.Instance.Check_HealthBar_UI_IsActive())
        {
            SaveManager.Instance.RestorePlayerStats(); // causes a unNoticed error
            //update healthbar
            var healthController = PlayerManager.Instance.GetComponentInChildren<IHealthController>();

            UIManager.Instance.UpdateHealtBar(healthController.GetCurrentHealth(), healthController.GetMaxHealth());
        }


    }
}

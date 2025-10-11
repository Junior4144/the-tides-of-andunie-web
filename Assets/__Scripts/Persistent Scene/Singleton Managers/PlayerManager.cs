using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private HealthController healthController;
    private MeleeController meleeController;


    [HideInInspector]
    public GameObject playerInstance;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        healthController = GetComponentInChildren<HealthController>();
        meleeController = GetComponentInChildren<MeleeController>();
    }
    private void Start()
    {
        SaveManager.Instance.InitializeDefaultSave();
    }
    public float GetHealth() => healthController.GetCurrentHealth();
    public float GetDamageAmount() => meleeController.GetDamageAmount();

    public void SetHealth(float value) => healthController.SetCurrentHealth(value);
    public void SetDamageAmount(float value) => meleeController.SetDamageAmount(value);
}
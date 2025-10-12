using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private IHealthController healthController;
    private MeleeController meleeController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        healthController = GetComponentInChildren<IHealthController>();
        meleeController = GetComponentInChildren<MeleeController>();
    }
    private void Start() =>
        SaveManager.Instance.InitializeDefaultSave();

    public float GetHealth() => healthController.GetCurrentHealth();
    public float GetDamageAmount() => meleeController.GetDamageAmount();

    public void SetHealth(float value) => healthController.SetCurrentHealth(value);
    public void SetDamageAmount(float value) => meleeController.SetDamageAmount(value);

    public void HandleDestroy() => GetComponent<DestroyController>().Destroy(0f);
}
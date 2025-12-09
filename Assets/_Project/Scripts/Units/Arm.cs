using UnityEngine;

public class Arm : HealthController
{
    [Header("Arm Stats")]
    [SerializeField] private SkeletonBossAttributes _attributes;

    private float maxHealth;
    private float currentHealth;
    private bool isBroken = false;
    private BossRoot bossRoot;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        if (_attributes != null)
        {
            maxHealth = _attributes.ArmHealth;
            currentHealth = maxHealth;
            Debug.Log($"[Arm - {gameObject.name}] Initialized with Health: {currentHealth}/{maxHealth}");
        }
        else
        {
            Debug.LogError("SkeletonBossAttributes is NULL - arm health not set!");
        }
    }

    public void Initialize(BossRoot root)
    {
        bossRoot = root;
    }

    public override void TakeDamage(float amount, DamageType damageType)
    {
        if (isBroken) return;

        float previousHealth = currentHealth;
        currentHealth -= amount;
        Debug.Log($"[Arm - {gameObject.name}] Took {amount} damage. Health: {previousHealth} → {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            BreakArm();
    }

    private void BreakArm()
    {
        if (isBroken)
        {
            Debug.LogWarning($"[Arm - {gameObject.name}] BreakArm called but arm is already broken!");
            return;
        }

        isBroken = true;
        Debug.Log($"[Arm - {gameObject.name}] ARM BROKEN!");

        // Detach from parent first
        transform.SetParent(null);

        // Enable physics so the arm "falls off"
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f; // Ensure gravity is enabled
        rb.mass = 1f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;

        // Inform boss
        if (bossRoot != null)
        {
            Debug.Log($"[Arm - {gameObject.name}] Notifying BossRoot that arm is broken");
            bossRoot.NotifyArmBroken();
        }
        else
        {
            Debug.LogError($"[Arm - {gameObject.name}] Cannot notify boss - bossRoot is NULL! Did you forget to call Initialize()?");
        }

        // Remove arm after 3 seconds
        Destroy(gameObject, 3f);
    }

}

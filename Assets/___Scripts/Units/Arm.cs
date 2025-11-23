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
        isBroken = true;

        transform.SetParent(null); 

        // Enable physics so the arm "falls off"
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-200, 200));

        // Disable further hits
        col.enabled = false;

        // Inform boss
        bossRoot.NotifyArmBroken();

        // Remove arm after 2 seconds
        Destroy(gameObject, 2f);
    }
}

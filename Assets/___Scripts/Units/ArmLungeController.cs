using UnityEngine;

public class ArmLungeController : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float attackRange = 5f;

    [Header("Lunge Settings")]
    public float windUpDistance = 0.5f;
    public float lungeDistance = 2f;
    public float windUpSpeed = 3f;
    public float lungeSpeed = 10f;
    public float retractSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip lungeSound;

    private Vector3 originalLocalPos;
    private AudioSource audioSource;

    private enum ArmState { Idle, WindUp, Lunge, Retract }
    private ArmState state = ArmState.Idle;

    private bool isAttacking = false;

    void Start()
    {
        originalLocalPos = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            // Try to add one if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log($"[ArmLunge - {gameObject.name}] Added AudioSource component");
        }
        
        if (audioSource != null)
        {
            audioSource.enabled = true;
            audioSource.playOnAwake = false;
            Debug.Log($"[ArmLunge - {gameObject.name}] AudioSource initialized - Enabled: {audioSource.enabled}, Volume: {audioSource.volume}");
        }
        else
        {
            Debug.LogError($"[ArmLunge - {gameObject.name}] Failed to get or create AudioSource!");
        }
        
        if (lungeSound == null)
        {
            Debug.LogWarning($"[ArmLunge - {gameObject.name}] No lunge sound assigned!");
        }
    }

    void Update()
    {
        float d = Vector2.Distance(player.position, transform.position);

        // Player in range → attack if not already attacking
        if (d <= attackRange && !isAttacking)
        {
            Debug.Log($"[ArmLunge - {gameObject.name}] Player in range, starting attack");
            isAttacking = true;
            state = ArmState.WindUp;
        }

        switch (state)
        {
            case ArmState.Idle:
                // do nothing until player enters range
                break;

            case ArmState.WindUp:
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    originalLocalPos - transform.up * windUpDistance,
                    windUpSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.localPosition, originalLocalPos - transform.up * windUpDistance) < 0.05f)
                {
                    state = ArmState.Lunge;
                    
                    // Play lunge sound
                    if (audioSource != null && lungeSound != null)
                    {
                        Debug.Log($"[ArmLunge - {gameObject.name}] Playing lunge sound: {lungeSound.name}");
                        audioSource.PlayOneShot(lungeSound);
                    }
                    else
                    {
                        Debug.LogWarning($"[ArmLunge - {gameObject.name}] Cannot play sound - AudioSource: {audioSource != null}, Clip: {lungeSound != null}");
                    }
                }

                break;

            case ArmState.Lunge:
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    originalLocalPos + transform.up * lungeDistance,
                    lungeSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.localPosition, originalLocalPos + transform.up * lungeDistance) < 0.05f)
                    state = ArmState.Retract;

                break;

            case ArmState.Retract:
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    originalLocalPos,
                    retractSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.localPosition, originalLocalPos) < 0.05f)
                {
                    state = ArmState.Idle;
                    isAttacking = false; // allow next attack
                }

                break;
        }
    }
}

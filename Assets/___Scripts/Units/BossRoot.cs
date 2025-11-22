using System.Collections;
using UnityEngine;

public class BossRoot : MonoBehaviour
{
    [Header("Boss References")]
    [SerializeField] private Arm leftArm;
    [SerializeField] private Arm rightArm;
    [SerializeField] private HeadRotateController headRotate;
    [SerializeField] private Transform player;

    [Header("Phase 2 Settings")]
    [SerializeField] private float triggerRadius = 5f;
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip loopingSoundEffect;
    [SerializeField] private float soundIntervalTime = 3f;
    [SerializeField] private AudioClip phase2Sound;
    [SerializeField] private AudioClip spinningSound;
    [SerializeField] private bool loopSpinningSound = true;

    private int brokenArms = 0;
    private bool phase2 = false;
    private Rigidbody2D rb;
    private Coroutine soundLoopCoroutine;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null)
        {
            audioSource.enabled = true;
        }
    }

    private void Start()
    {
        // Auto-locate player
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }

        // Assign root to arms
        if (leftArm)  leftArm.Initialize(this);
        if (rightArm) rightArm.Initialize(this);

        // Start sound loop
        if (loopingSoundEffect != null)
        {
            Debug.Log($"[BossRoot] Starting sound loop with interval: {soundIntervalTime}s");
            soundLoopCoroutine = StartCoroutine(PlaySoundLoop());
        }
        else
        {
            Debug.LogWarning("[BossRoot] No looping sound effect assigned!");
        }

        if (audioSource == null)
        {
            Debug.LogError("[BossRoot] No AudioSource component found!");
        }
    }

    private void Update()
    {
        if (!phase2 || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Only chase if player enters the trigger bubble
        if (dist <= triggerRadius)
        {
            headRotate.canSpin = true;

            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );
        }
    }

    public void NotifyArmBroken()
    {
        brokenArms++;

        if (brokenArms >= 2 && !phase2)
            EnterPhase2();
    }

    private void EnterPhase2()
    {
        phase2 = true;
        headRotate.spinSpeed = 1200f;
        headRotate.canSpin = true;
        
        // Play phase 2 transition sound
        if (audioSource != null && phase2Sound != null)
        {
            Debug.Log($"[BossRoot] Playing phase 2 sound: {phase2Sound.name}");
            audioSource.PlayOneShot(phase2Sound);
        }
        
        // Start spinning sound
        if (audioSource != null && spinningSound != null)
        {
            if (loopSpinningSound)
            {
                audioSource.clip = spinningSound;
                audioSource.loop = true;
                audioSource.Play();
                Debug.Log($"[BossRoot] Started looping spinning sound: {spinningSound.name}");
            }
            else
            {
                audioSource.PlayOneShot(spinningSound);
                Debug.Log($"[BossRoot] Playing spinning sound once: {spinningSound.name}");
            }
        }
    }

    private IEnumerator PlaySoundLoop()
    {
        while (true)
        {
            if (audioSource != null && loopingSoundEffect != null)
            {
                Debug.Log($"[BossRoot] Playing sound effect: {loopingSoundEffect.name}");
                audioSource.PlayOneShot(loopingSoundEffect);
            }
            else
            {
                Debug.LogWarning($"[BossRoot] Cannot play sound - AudioSource: {audioSource != null}, Clip: {loopingSoundEffect != null}");
            }
            
            yield return new WaitForSeconds(soundIntervalTime);
        }
    }

    private void OnDestroy()
    {
        // Stop sound loop when boss is destroyed
        if (soundLoopCoroutine != null)
        {
            StopCoroutine(soundLoopCoroutine);
        }
    }
}

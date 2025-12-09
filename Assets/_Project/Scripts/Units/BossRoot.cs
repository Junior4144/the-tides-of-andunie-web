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
    
    [Header("Phase 2 Spinning Audio")]
    [SerializeField] private AudioClip spinningSound;
    [Tooltip("If false, script will not control the spinning audio")]
    [SerializeField] private bool useScriptControlledSpinningAudio = true;
    [Tooltip("Delay in seconds before starting spinning audio (useful to wait for phase 2 sound to finish)")]
    [SerializeField] private float spinningAudioDelay = 0.1f;

    private int brokenArms = 0;
    private bool phase2 = false;
    private Rigidbody2D rb;
    private Coroutine soundLoopCoroutine;
    private AudioSource audioSource;
    private AudioSource spinningAudioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        // Create a dedicated AudioSource for seamless looping with optimized settings
        spinningAudioSource = gameObject.AddComponent<AudioSource>();
        spinningAudioSource.playOnAwake = false;
        spinningAudioSource.loop = true;
        spinningAudioSource.priority = 0; // Highest priority to prevent interruption
        spinningAudioSource.bypassEffects = true; // Skip audio effects for lower latency
        spinningAudioSource.bypassListenerEffects = true;
        spinningAudioSource.bypassReverbZones = true;
        
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
        
        // Start spinning sound if script-controlled
        if (useScriptControlledSpinningAudio && spinningAudioSource != null)
        {
            StartCoroutine(StartSpinningAudioDelayed());
        }
    }

    private IEnumerator StartSpinningAudioDelayed()
    {
        // Wait for specified delay (useful to avoid conflict with phase 2 transition sound)
        yield return new WaitForSeconds(spinningAudioDelay);
        
        if (spinningAudioSource != null && spinningSound != null)
        {
            spinningAudioSource.clip = spinningSound;
            spinningAudioSource.Play();
            Debug.Log($"[BossRoot] Started seamless looping spinning sound: {spinningSound.name}");
        }
        else
        {
            Debug.LogWarning($"[BossRoot] Cannot play spinning sound - AudioSource: {spinningAudioSource != null}, Clip: {spinningSound != null}");
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

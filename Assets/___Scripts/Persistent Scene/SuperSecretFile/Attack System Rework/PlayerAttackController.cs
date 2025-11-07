using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerRoot;
    [SerializeField] PlayerAnimator animator;
    [SerializeField] AudioClip attackSound;
    [SerializeField] GameObject hitEffectPrefab;

    [Header("Settings")]
    [SerializeField] float attackDuration = 0.5f;
    [SerializeField] float damageDelay = 0f;

    AudioSource audioSrc;
    Rigidbody2D rb;
    HashSet<Collider2D> hitEnemies = new();

    bool isAttacking;

    public bool IsAttacking => isAttacking;
    public float AttackDuration => attackDuration;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        rb = playerRoot.GetComponent<Rigidbody2D>();
        isAttacking = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
            StartAttack();

    }

    void StartAttack()
    {
        isAttacking = true;
        PlayAttackAnimation();
    }

    void PlayAttackAnimation()
    {
        animator?.TriggerAttack();
        if (attackSound) audioSrc.PlayOneShot(attackSound, 0.4f);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        hitEnemies.Clear();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isAttacking) return;
        if (hitEnemies.Contains(col)) return;

        
        if (col.TryGetComponent(out IHealthController health))
        {
            hitEnemies.Add(col);
            StartCoroutine(DealDamage(health));
            SpawnHitEffect(col.transform.position);
        } 
    }

    IEnumerator DealDamage(IHealthController health)
    {
        yield return new WaitForSeconds(damageDelay);
        health.TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
    }

    void SpawnHitEffect(Vector2 enemyPos)
    {
        Vector2 playerPos = rb.transform.position;
        Vector2 dir = (enemyPos - playerPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle + 90f);
        Instantiate(hitEffectPrefab, enemyPos, rot);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;
    [SerializeField] private float damageDelay = 0;
    [SerializeField] private string _layerName;
    [SerializeField] private float _animDuration;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] Slider BowPowerSlider;

    private bool _isAttacking = false;
    private AudioSource _audioSource;

    public bool IsAttacking => _isAttacking;
    public float AttackDuration => _animDuration;

    [Range(0f, 10f)]
    [SerializeField] float BowPower;

    [Range(0f, 3f)]
    [SerializeField] float MaxBowCharge; 

    float BowCharge;

    bool CanAttack = true;
    public float RotationSpeed = 1f;

    private bool isSwinging = false;

    private void Awake()
    {
        _isAttacking = false;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && CanAttack)
        {
            _isAttacking = true;
            ChargeBow();
            RotateHand();
        }
        else if (Input.GetMouseButtonUp(0) && CanAttack)
        {
            FireBow();
            RotateHand();
        }
        else
        {
            
            if (BowCharge > 0f)
                BowCharge -= 10f * Time.deltaTime;
            else
            {
                BowCharge = 0f;
                CanAttack = true;
            }
            BowPowerSlider.value = BowCharge;
        }

        if (isSwinging)
        {
            RotateHand();
        }
    }

    void ChargeBow()
    {
        BowCharge += Time.deltaTime * 3f;

        BowPowerSlider.value = BowCharge;

        if (BowCharge > MaxBowCharge)
        {
            BowPowerSlider.value = MaxBowCharge;
        }
    }
    private void FireBow()
    {
        CanAttack = false;

        if (BowCharge > MaxBowCharge) BowCharge = MaxBowCharge;

        float ArrowSpeed = BowCharge + BowPower;

        float angle = Utility.AngleTowardsMouse(gameObject.transform.position);
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));

        PlayAttackAnimation();
    }

    private void RotateHand()
    {
        float targetAngle = Utility.AngleTowardsMouse(playerRoot.position);
        float currentAngle = playerRoot.gameObject.GetComponent<Rigidbody2D>().rotation;

        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed);

        playerRoot.gameObject.GetComponent<Rigidbody2D>().MoveRotation(smoothAngle);
    }

    private void PlayAttackAnimation()
    {
        if (_animator)
            _animator.TriggerAttack();

        if (_attackSound)
            _audioSource.PlayOneShot(_attackSound, 0.4f);

        isSwinging = true;
       StartCoroutine(SwingingAttack());
    }

    private IEnumerator SwingingAttack()
    {
        yield return new WaitForSeconds(_animDuration);
        isSwinging = false;
        _isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAttacking) return;
        if (col.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;

        if (col.TryGetComponent(out IHealthController health))
            StartCoroutine(DealDamage(health));
    }

    private IEnumerator DealDamage(IHealthController health)
    {
        yield return new WaitForSeconds(damageDelay);
        health.TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
    }

}
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
    [SerializeField] private float attackTurnSpeed = 15f;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private float rotationThreshold = 1f; // how close to target angle before stopping
    [SerializeField] Slider BowPowerSlider;


    private bool _isAttacking = false;
    private float targetAngle;
    private bool _isRotating = false;
    private AudioSource _audioSource;
    private bool _queuedAttack = false;

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
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //// When player clicks
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (!_isAttacking && !_isRotating)
        //    {
        //        // Start attack normally
        //        //SetTargetDirection();
        //        //StartRotateToTarget();
        //    }
        //    else if (_isAttacking && !_queuedAttack)
        //    {
        //        // Queue one attack only
        //        _queuedAttack = true;
        //    }
        //}

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

        //ArrowProjectile arrow = Instantiate(ArrowPrehab, gameObject.transform.position, rot).GetComponent<ArrowProjectile>();
        //arrow.ArrowVelocity = ArrowSpeed;
        //arrow.power = BowCharge;

        PlayAttackAnimation();
    }

    private void RotateHand()
    {
        float targetAngle = Utility.AngleTowardsMouse(playerRoot.position);
        float currentAngle = playerRoot.gameObject.GetComponent<Rigidbody2D>().rotation;

        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed);

        playerRoot.gameObject.GetComponent<Rigidbody2D>().MoveRotation(smoothAngle);
    }

    //private void SetTargetDirection()
    //{
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 direction = (mousePos - playerRoot.position);
    //    targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    //}

    //private void StartRotateToTarget()
    //{
    //    _isAttacking = true;
    //    if (!_isRotating)
    //        StartCoroutine(RotateToTargetCoroutine());
    //}

    //private IEnumerator RotateToTargetCoroutine()
    //{
    //    _isRotating = true;

    //    while (true)
    //    {
    //        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);
    //        playerRoot.rotation = Quaternion.RotateTowards(
    //            playerRoot.rotation,
    //            targetRot,
    //            attackTurnSpeed * Time.deltaTime
    //        );

    //        float angleDiff = Quaternion.Angle(playerRoot.rotation, targetRot);
    //        if (angleDiff < rotationThreshold)
    //            break;

    //        yield return null;
    //    }

    //    _isRotating = false;
    //    OnRotationComplete();
    //}

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
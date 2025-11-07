using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBowAttackController : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;
    [SerializeField] private string _layerName;
    [SerializeField] private float _animDuration;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] Slider BowPowerSlider;

    private AudioSource _audioSource;
    private bool _isAttacking = false;

    public bool IsAttacking => _isAttacking;
    public float AttackDuration => _animDuration;

    public Transform firePoint;
    public GameObject arrowSprite;

    public float RotationSpeed = 1f;


    float BowCharge;

    bool CanFire = true;

    private bool inputEnabled = false;

    [Range(0f, 10f)]
    [SerializeField] float BowPower;

    [Range(0f, 3f)]
    [SerializeField] float MaxBowCharge;

    [SerializeField] GameObject ArrowPrehab;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _isAttacking = false;
        StartCoroutine(EnableInputAfterDelay(0.2f));
    }

    private IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        inputEnabled = true;
    }


    private void Start()
    {
        BowPowerSlider.value = 0f;
        BowPowerSlider.maxValue = MaxBowCharge;
    }

    private void Update()
    {
        if (!inputEnabled) return;

        if (Input.GetMouseButton(0) && CanFire)
        {
            _isAttacking = true;
            ChargeBow();
            RotateHand();
        }
        else if (Input.GetMouseButtonUp(0) && CanFire)
            FireBow();
        else
        {
            if (BowCharge > 0f)
                BowCharge -= 10f * Time.deltaTime;
            else
            {
                BowCharge = 0f;
                CanFire = true;
            }
            BowPowerSlider.value = BowCharge;
        }
    }
    void ChargeBow()
    {
        arrowSprite.SetActive(true);

        BowCharge += Time.deltaTime * 3f;

        BowPowerSlider.value = BowCharge;

        if (BowCharge > MaxBowCharge)
        {
            BowPowerSlider.value = MaxBowCharge;
        }
    }
    private void FireBow()
    {
        CanFire = false;

        if (BowCharge > MaxBowCharge) BowCharge = MaxBowCharge;

        float ArrowSpeed = BowCharge + BowPower * 3f;

        float angle = Utility.AngleTowardsMouse(gameObject.transform.position);
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));

        ArrowProjectile arrow = Instantiate(ArrowPrehab, gameObject.transform.position, rot).GetComponent<ArrowProjectile>();
        arrow.ArrowVelocity = ArrowSpeed;
        arrow.power = BowCharge;

        _isAttacking = false;
        arrowSprite.SetActive(false);

    }

    private void RotateHand()
    {
        float targetAngle = Utility.AngleTowardsMouse(playerRoot.position);
        float currentAngle = playerRoot.gameObject.GetComponent<Rigidbody2D>().rotation;

        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed);

        playerRoot.gameObject.GetComponent<Rigidbody2D>().MoveRotation(smoothAngle);
    }

}

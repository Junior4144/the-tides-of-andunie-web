using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private float occurAfterVelocity = 5f;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float footStepSpread = 0.2f;
    
    [SerializeField] private Rigidbody2D playerRigidbody;
    private ParticleSystem particles;
    private float counter = 0f;
    private bool isLeftFoot = true;

    void Start()
    {

        particles = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (playerRigidbody == null) return;

        if (playerRigidbody.linearVelocity.magnitude >= occurAfterVelocity)
        {
            counter += Time.deltaTime;

            if (counter >= spawnInterval)
            {
                SpawnFootstep();
                counter = 0f;
                isLeftFoot = !isLeftFoot;
            }
        }
        else
            counter = 0f;
    }
    
    void SpawnFootstep()
    {
        float side = isLeftFoot ? -footStepSpread : footStepSpread;
        transform.localPosition = new Vector3(side, 0, 0);
        particles.Play();
    }
}
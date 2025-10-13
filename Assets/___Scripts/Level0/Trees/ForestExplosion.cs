using Unity.Cinemachine;
using UnityEngine;

public class ForestExplosion : MonoBehaviour
{
    public GameObject explosion;

    [SerializeField]
    private AudioClip _explosionSound;

    private Camera cam;

    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("CannonBall"))
        {
            Debug.Log("tree Hit");
            HandleTreeObject(collision);
        }

    }
    private void HandleTreeObject(Collider2D collision)
    {
        //only activate in border distancein border distance
        cam = Camera.main;
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        bool inView = (viewPos.x > 0 && viewPos.x < 1 &&
               viewPos.y > 0 && viewPos.y < 1 &&
               viewPos.z > 0);

        if (inView)
        {
            Transform locationCannonBall = collision.transform;
            SpawnExplosion(locationCannonBall);
            SpawnExplosionSound(locationCannonBall);
            HandleTreeCameraShake();
        }


    }

    public void SpawnExplosion(Transform locationCannonBall) => Instantiate(explosion, locationCannonBall.position, Quaternion.identity);
    public void SpawnExplosionSound(Transform locationCannonBall)
    {
        SoundFxManager.instance.PlayerSoundFxClip(_explosionSound, locationCannonBall, 1f);
    }
    public void HandleTreeCameraShake() =>
        CameraShakeManager.instance.CameraShake(_impulseSource);


    public bool CheckCameraAllBoundary(Vector2 screenPosition, float padding = 0f)
    {
        return screenPosition.x > -padding &&
               screenPosition.x < Screen.width + padding &&
               screenPosition.y > -padding &&
               screenPosition.y < Screen.height + padding;
    }
}


using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [SerializeField]
    private AudioClip shakeImpactSound;

    private void Awake() =>
        instance = this;

    [Header("Shake Settings")]
    public float defaultDuration = 0.5f;
    public float defaultMagnitude = 0.3f;

    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();
        PlayImpactSound();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + offsetX, originalPos.y + offsetY, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }

    private void PlayImpactSound()
    {
        if (shakeImpactSound != null)
        {
            AudioSource.PlayClipAtPoint(shakeImpactSound, transform.position, 0.1f);
        }
        else
        {
            Debug.LogError("ShakeImpactSound is Null. Playing no Sound");
        }
    }

}

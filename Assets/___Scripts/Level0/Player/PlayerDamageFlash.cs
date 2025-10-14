using UnityEngine;
using System.Collections;

public class FlashOnDamage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private Color originalColor;

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}

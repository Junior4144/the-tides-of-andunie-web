using UnityEngine;
using System.Collections;

public class FlashOnDamage : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    [SerializeField] float _flashDuration = 0.2f;
    [SerializeField] Color _flashColor = Color.white;

    Material _material;
    static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
    static readonly int FlashColor = Shader.PropertyToID("_FlashColor");

    void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _material = _spriteRenderer.material;

        _material.SetFloat(FlashAmount, 0);
        _material.SetColor(FlashColor, _flashColor);
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        _material.SetFloat(FlashAmount, 1);
        yield return new WaitForSeconds(_flashDuration);
        _material.SetFloat(FlashAmount, 0);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _healthBarForegroundImage;
    [SerializeField] private Image _healthBarDamageIndicator;
    [SerializeField] private float _damageIndicatorSpeed = 0.5f;
    [SerializeField] private float _delayBeforeShrink = 0.1f;

    private Coroutine _damageRoutine;

    public void UpdateHealthBar(HealthController healthController)
    {
        float targetFill = healthController.GetPercentHealth();
        _healthBarForegroundImage.fillAmount = targetFill; // Handles Red Bar

        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);

        _damageRoutine = StartCoroutine(AnimateDamageBar(targetFill)); // Handle White Transparent Bar
    }

    private IEnumerator AnimateDamageBar(float targetFill)
    {
        yield return new WaitForSeconds(_delayBeforeShrink);

        float startFill = _healthBarDamageIndicator.fillAmount;
        float elapsed = 0f;

        while (elapsed < _damageIndicatorSpeed)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _damageIndicatorSpeed;

            UpdateDamageIndicatorFill(startFill, targetFill, progress);
            yield return null;
        }
        _healthBarDamageIndicator.fillAmount = targetFill;
    }

    private void UpdateDamageIndicatorFill(float startFill, float targetFill, float progress) =>
        _healthBarDamageIndicator.fillAmount = Mathf.Lerp(startFill, targetFill, progress);
}

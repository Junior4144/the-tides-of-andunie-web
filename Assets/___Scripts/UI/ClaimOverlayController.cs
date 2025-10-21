using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ClaimOverlayController : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public TMP_Text claimText;
    public Image iconImage;

    [Header("Timing")]
    public float fadeDuration = 0.4f;
    public float displayDuration = 1.8f;
    public float popScale = 1.25f;

    private Vector3 originalScale;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (iconImage != null)
            originalScale = iconImage.rectTransform.localScale;
    }

    public void ShowMessage(string itemName, Sprite icon)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(itemName, icon));
    }
    
    private IEnumerator ShowMessageRoutine(string itemName, Sprite icon)
    {
        Debug.Log($"[ClaimOverlay] Icon: {(icon != null ? icon.name : "NULL")}");
        Debug.Log($"[ClaimOverlay] Showing message: {itemName}");
        claimText.text = $"Congratulations! You claimed {itemName}!";

        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = (icon != null);
            iconImage.rectTransform.localScale = Vector3.zero; // start hidden
        }

        // Activate input blocking during the animation
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);

            if (iconImage != null)
            {
                float scale = Mathf.Lerp(0, popScale, t / fadeDuration);
                iconImage.rectTransform.localScale = new Vector3(scale, scale, 1);
            }

            yield return null;
        }

        // Slight “pop” settle effect
        if (iconImage != null)
        {
            float s = 0f;
            while (s < 0.2f)
            {
                s += Time.deltaTime;
                iconImage.rectTransform.localScale = Vector3.Lerp(
                    new Vector3(popScale, popScale, 1),
                    originalScale,
                    s / 0.2f
                );
                yield return null;
            }
        }

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}

using UnityEngine;
using DG.Tweening;

public class PopupAutoDestroy : MonoBehaviour
{
    [Header("Scale Settings")]
    public float startScale = 0.2f;
    public float endScale = 1f;
    public float scaleDuration = 0.25f;

    [Header("Fade Settings")]
    public float lifetimeBeforeFade = 1f;
    public float fadeDuration = 0.4f;

    private CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();
        
        transform.localScale = Vector3.one * startScale;
    }

    private void Start()
    {
        transform.DOScale(endScale, scaleDuration)
                 .SetEase(Ease.OutBack);
        
        cg.DOFade(0f, fadeDuration)
          .SetDelay(lifetimeBeforeFade)
          .OnComplete(() => Destroy(gameObject));
    }
}
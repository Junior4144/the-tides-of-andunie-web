using DG.Tweening;
using UnityEngine;

public class LargeUIAnimation : MonoBehaviour
{
    public enum AnimType
    {
        SlideOnly,
        PopUpEffect
    }

    [Header("Animation Type")]
    public AnimType AnimationType = AnimType.SlideOnly;

    [Header("Settings SlideOnly")]
    public float distance = 20f;
    public float duration = 0.2f;
    public Ease ease = Ease.OutCubic;

    CanvasGroup cg;
    RectTransform rt;
    Vector2 basePos;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        basePos = rt.anchoredPosition;

        cg = GetComponent<CanvasGroup>();
        if (!cg) cg = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        rt.DOKill();
        cg.DOKill();

        switch (AnimationType)
        {
            case AnimType.SlideOnly:
                DoSlideOnly();
                break;
            case AnimType.PopUpEffect:
                PopUpEffect();
                break;
        }
    }

    void DoSlideOnly()
    {
        Vector2 start = basePos + new Vector2(0, -distance);
        rt.anchoredPosition = start;
        cg.alpha = 1f;

        rt.DOAnchorPosY(basePos.y, duration).SetEase(ease);
    }

    void PopUpEffect()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        rt.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
    }
}
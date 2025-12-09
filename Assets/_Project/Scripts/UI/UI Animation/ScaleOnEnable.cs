using DG.Tweening;
using UnityEngine;

public class ScaleOnEnable : MonoBehaviour
{
    private RectTransform rt;
    public bool IsAnimating { get; private set; }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        IsAnimating = true;
        rt.localScale = Vector3.zero;

        rt.DOScale(1f, 0.35f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => IsAnimating = false);
    }

    private void OnDisable()
    {
        rt.DOKill();
        IsAnimating = false;
    }

    public void HideWithScale()
    {
        IsAnimating = true;

        rt.DOScale(0f, 0.25f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                IsAnimating = false;
                gameObject.SetActive(false);
            });
    }
}
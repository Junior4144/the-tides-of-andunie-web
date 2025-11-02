using DG.Tweening;
using UnityEngine;

public class ScaleOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        rt.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
    }

    void OnDisable()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOScale(0f, 0.35f).SetEase(Ease.InBack);
    }
}
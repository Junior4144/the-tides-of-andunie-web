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
}
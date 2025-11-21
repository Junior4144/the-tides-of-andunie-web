using UnityEngine;
using DG.Tweening;

public class PopupTextTween : MonoBehaviour
{
    void OnEnable()
    {
        // Start tiny
        transform.localScale = Vector3.zero;

        // Expand with a pop
        transform.DOScale(1f, 0.25f)
            .SetEase(Ease.OutBack);
    }
}
using UnityEngine;
using DG.Tweening;

public class PopupTextTween : MonoBehaviour
{
    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        
        transform.DOScale(1f, 0.25f)
            .SetEase(Ease.OutBack);
    }
}
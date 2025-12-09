using UnityEngine;
using DG.Tweening;

public class ScalePulse : MonoBehaviour
{
    [SerializeField] private float scaleAmount = 1.1f;

    private Tween pulseTween;

    void Awake()
    {
        pulseTween = transform.DOScale(scaleAmount, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void OnDestroy()
    {
        pulseTween?.Kill();
    }
}
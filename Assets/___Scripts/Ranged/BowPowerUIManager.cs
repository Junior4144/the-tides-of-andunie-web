using UnityEngine;
using UnityEngine.UI;

public class BowPowerUIManager : MonoBehaviour
{

    public static BowPowerUIManager instance;

    [SerializeField] public Slider slider;
    [SerializeField] RectTransform sliderRoot;
    [SerializeField] Image sliderImageFill;
    [SerializeField] private RectTransform _canvasRect;

    [Header("Offset Settings")]
    [SerializeField] Vector2 positionOffset = new Vector2(50f, -50f); // adjust in inspector
    
    private Color defaultColor;

    private void Awake()
    {
        instance = this;
        ColorUtility.TryParseHtmlString("#FCCE00", out defaultColor);
    }

    private void Update()
    {
        if (sliderRoot == null) return;
        if(WeaponManager.Instance == null) return;

        if (WeaponManager.Instance.IsAbilityAiming)
        {
            sliderImageFill.color = Color.orange;
        }
        else
        {
            sliderImageFill.color = defaultColor;
        }

        UpdatePosition();
    }
    
    void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect,
            Input.mousePosition,
            null,
            out Vector2 localPos
        );

        sliderRoot.anchoredPosition = localPos + positionOffset;
    }
}

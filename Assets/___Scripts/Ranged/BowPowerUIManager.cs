using UnityEngine;
using UnityEngine.UI;

public class BowPowerUIManager : MonoBehaviour
{

    public static BowPowerUIManager instance;

    [SerializeField] public Slider slider;
    [SerializeField] RectTransform sliderRoot;
    [SerializeField] Image sliderImageFill;

    [Header("Offset Settings")]
    [SerializeField] Vector2 positionOffset = new Vector2(50f, -50f); // adjust in inspector
    Color defaultColor;

    private void Awake()
    {
        instance = this;
        ColorUtility.TryParseHtmlString("#FCCE00", out defaultColor);
    }

    void Update()
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

            sliderRoot.position = Input.mousePosition + (Vector3)positionOffset;
    }

}

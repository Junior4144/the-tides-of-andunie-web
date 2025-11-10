using UnityEngine;
using UnityEngine.UI;

public class BowPowerUIManager : MonoBehaviour
{

    public static BowPowerUIManager instance;

    [SerializeField] public Slider slider;
    [SerializeField] RectTransform sliderRoot;

    [Header("Offset Settings")]
    [SerializeField] Vector2 positionOffset = new Vector2(50f, -50f); // adjust in inspector

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (sliderRoot == null) return;

        // Add offset to the mouse position
        sliderRoot.position = Input.mousePosition + (Vector3)positionOffset;
    }

}

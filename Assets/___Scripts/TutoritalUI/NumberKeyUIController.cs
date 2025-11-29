using UnityEngine;
using UnityEngine.UI;

public class NumberKeyUIController : MonoBehaviour
{
    [Header("Key 1 Sprites")]
    [SerializeField] private Image key1Image;
    [SerializeField] private Sprite key1Normal;
    [SerializeField] private Sprite key1Pressed;

    [Header("Key 2 Sprites")]
    [SerializeField] private Image key2Image;
    [SerializeField] private Sprite key2Normal;
    [SerializeField] private Sprite key2Pressed;

    void Update()
    {
        // Key 1
        if (Input.GetKey(KeyCode.Alpha1))
            key1Image.sprite = key1Pressed;
        else
            key1Image.sprite = key1Normal;

        // Key 2
        if (Input.GetKey(KeyCode.Alpha2))
            key2Image.sprite = key2Pressed;
        else
            key2Image.sprite = key2Normal;
    }
}

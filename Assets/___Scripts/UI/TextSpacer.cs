using UnityEngine;
using TMPro;

public class TextSpacer : MonoBehaviour
{
    void Start()
    {
        var textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = textMeshPro.text.Replace(" ", "    ");
        }
    }
}

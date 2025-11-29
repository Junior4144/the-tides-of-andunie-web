using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyPressController : MonoBehaviour
{
    [System.Serializable]
    public struct KeyPressUI
    {
        public Image image;
        public Sprite unpressedSprite;
        public Sprite pressedSprite;
        public KeyCode key;

        public readonly void UpdateSprite()
        {
            image.sprite = Input.GetKey(key) ? pressedSprite : unpressedSprite;
        }
    }

    [SerializeField]
    private List<KeyPressUI> keyPressUIs = new();

    void Update() 
    {
        foreach (KeyPressUI key in keyPressUIs.Where(k => k.image != null))
            key.UpdateSprite();
    }
        
}

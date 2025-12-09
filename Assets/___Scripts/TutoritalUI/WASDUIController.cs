using UnityEngine;
using UnityEngine.UI;

public class WASDUIController : MonoBehaviour
{
    [Header("W Key Sprites")]
    public Image wImage;
    public Sprite wNormal;
    public Sprite wPressed;

    [Header("A Key Sprites")]
    public Image aImage;
    public Sprite aNormal;
    public Sprite aPressed;

    [Header("S Key Sprites")]
    public Image sImage;
    public Sprite sNormal;
    public Sprite sPressed;

    [Header("D Key Sprites")]
    public Image dImage;
    public Sprite dNormal;
    public Sprite dPressed;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            wImage.sprite = wPressed;
        else
            wImage.sprite = wNormal;
        
        if (Input.GetKey(KeyCode.A))
            aImage.sprite = aPressed;
        else
            aImage.sprite = aNormal;
        
        if (Input.GetKey(KeyCode.S))
            sImage.sprite = sPressed;
        else
            sImage.sprite = sNormal;
        
        if (Input.GetKey(KeyCode.D))
            dImage.sprite = dPressed;
        else
            dImage.sprite = dNormal;
    }
}

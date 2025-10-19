using UnityEngine;

public class LevelSelectionMenu : MonoBehaviour
{
    private bool Clicked = false;

    public void ButtonClicked()
    {
        if (Clicked) return;

        Clicked = true;
        Debug.Log("[LevelSelectionMenu] Button Clicked");

        LevelSelectionUIManager.Instance.ButtonClicked();
    }
}

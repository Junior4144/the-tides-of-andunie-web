using UnityEngine;

public class LSEnterMenu : MonoBehaviour
{
    private bool Clicked = false;

    public void ButtonClicked()
    {
        if (Clicked) return;

        Clicked = true;
        Debug.Log("[LevelSelectionMenu] Button Clicked");

        LSUIManager.Instance.ButtonClicked();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Clicked) return;

            Clicked = true;
            Debug.Log("[LevelSelectionMenu] Button Clicked");

            LSUIManager.Instance.ButtonClicked();
        }
    }
}

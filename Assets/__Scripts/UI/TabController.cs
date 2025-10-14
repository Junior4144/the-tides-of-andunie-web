using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public UnityEngine.UI.Image[] tabImages;
    public GameObject[] pages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        Debug.Log("Activating tab: " + tabNo);

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        }
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
    }

}

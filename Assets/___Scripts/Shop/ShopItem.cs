using UnityEngine;

public class ShopItem : MonoBehaviour
{
    //Will require a Whole Shop Item script that takes in IInventory Interface
    [SerializeField]
    private GameObject canvas;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            canvas.SetActive(!canvas.activeInHierarchy);
        }
    }
    public void OnClick()
    {
        Debug.Log("[ShopItem] is being clicked");
        InventoryManager.Instance.RemoveCoins(1);
    }
}

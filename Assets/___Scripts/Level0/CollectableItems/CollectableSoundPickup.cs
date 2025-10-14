using UnityEngine;

public class CollectableSoundPickup : MonoBehaviour
{
    public GameObject _Sound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && PlayerManager.Instance.GetPercentHealth() < 0.98)
        {
            Debug.Log("eating noise");
            Instantiate(_Sound, transform.position, Quaternion.identity);
        }
    }
}

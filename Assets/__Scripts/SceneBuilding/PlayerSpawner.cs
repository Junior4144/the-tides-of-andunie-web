using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = PlayerManager.Instance.gameObject;

        player.transform.position = transform.position;
    }
}

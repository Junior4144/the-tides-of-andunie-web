using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");

        player.transform.position = transform.position;
    }
}

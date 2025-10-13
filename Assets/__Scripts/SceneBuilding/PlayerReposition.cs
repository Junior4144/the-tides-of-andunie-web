using UnityEngine;

public class PlayerReposition : MonoBehaviour
{
    void Start()
    {
        GameObject player = PlayerManager.Instance.gameObject;
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}

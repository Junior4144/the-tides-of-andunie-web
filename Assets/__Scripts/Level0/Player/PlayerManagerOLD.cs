using UnityEngine;

public class PlayerManagerOLD : MonoBehaviour
{
    public static PlayerManagerOLD Instance { get; private set; }
    public GameObject playerPrefab;
    public GameObject Player { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (Player == null)
        {
            Player = Instantiate(playerPrefab);
            DontDestroyOnLoad(Player);
        }
    }
}
using UnityEngine;

public class RestartSington : MonoBehaviour
{
    public static RestartSington Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }



}

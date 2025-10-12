using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

    }
}
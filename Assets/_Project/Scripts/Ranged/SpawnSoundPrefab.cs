using UnityEngine;

public class SpawnSoundPrefab : MonoBehaviour
{
    [SerializeField] private GameObject soundPrefab;

    private void Start()
    {
        Instantiate(soundPrefab, transform.position, Quaternion.identity);
    }
}

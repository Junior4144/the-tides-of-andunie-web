using UnityEngine;

public class DestroyController : MonoBehaviour
{
    public void Destroy(float delay)
    {
        Destroy(gameObject, delay);
    }
}

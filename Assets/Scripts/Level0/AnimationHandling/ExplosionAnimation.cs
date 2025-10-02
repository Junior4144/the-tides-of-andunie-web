using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    public void ExplosionDone()
    {
        Debug.Log("active");
        Destroy(gameObject);
    }
}

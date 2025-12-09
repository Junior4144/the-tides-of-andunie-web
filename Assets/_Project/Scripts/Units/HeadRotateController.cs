using UnityEngine;

public class HeadRotateController: MonoBehaviour
{
   public float spinSpeed = 0f;
   public bool canSpin = false;  // NEW — controlled by BossRoot

    void Update()
    {
        if (!canSpin) return;
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}


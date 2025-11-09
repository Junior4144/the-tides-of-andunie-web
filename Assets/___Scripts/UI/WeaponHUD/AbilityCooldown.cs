using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownController : MonoBehaviour
{
    public Image cooldownImage;
    public float cooldownTime = 5f;
    private float timer;
    private bool isCoolingDown;


    void Update()
    {
        if (isCoolingDown)
        {
            timer -= Time.deltaTime;
            cooldownImage.fillAmount = timer / cooldownTime;

            if (timer <= 0f)
            {
                isCoolingDown = false;
                cooldownImage.fillAmount = 0f;
                gameObject.SetActive(true);
            }
        }
    }

    public void ActivateAbility()
    {
        if (!isCoolingDown)
        {
            // trigger ability logic here
            isCoolingDown = true;
            timer = cooldownTime;
            cooldownImage.fillAmount = 1f;
        }
    }
}

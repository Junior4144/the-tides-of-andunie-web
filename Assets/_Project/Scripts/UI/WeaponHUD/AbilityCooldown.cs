using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownController : MonoBehaviour
{
    public Image cooldownImage;
    private float cooldownTime;
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
                gameObject.SetActive(false);
            }
        }
    }

    public void ActivateAbility(float duration)
    {
        if (!isCoolingDown)
        {
            isCoolingDown = true;
            cooldownTime = duration;
            timer = cooldownTime;
            cooldownImage.fillAmount = 1f;
        }
    }
}
